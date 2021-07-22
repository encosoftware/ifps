import json
from functools import partial
from pathlib import Path
from typing import List, Callable, Optional, Dict
import numpy as np
import logging

from display import cv2_display
from axis_camera import AxisCamera
from qr_reader import check_input_img, detect_qr_codes, unpack_qr_data, preprocess_image
from message_sender import MessageSender


logging.basicConfig(format='%(asctime)s.%(msecs)03d %(levelname)-6s:: %(message)s',
                    datefmt='%Y-%m-%d %H:%M:%S')
logger = logging.getLogger(__name__)


class Controller:
    def __init__(self,
                 image_provider_fn: Callable[[], np.ndarray],
                 display_fn: Optional[Callable[[np.ndarray, List], None]] = None,
                 image_preprocessor_fn: Optional[Callable[[np.ndarray], np.ndarray]] = None,
                 result_handler_fn: Optional[Callable[[Dict[str, str], Callable[[dict], str]], None]] = None):
        self.image_provider_fn = image_provider_fn
        self.display_fn = display_fn
        self.image_preprocessor_fn = image_preprocessor_fn
        self.result_handler_fn = result_handler_fn
    
    def single_work_cycle(self):
        img = self.image_provider_fn()
        if img is None:
            logger.warning(f"Didn't receive any image from the provider. Aborting cycle.")
            return
        if self.image_preprocessor_fn:
            img = self.image_preprocessor_fn
        if not check_input_img(img):
            return
        detected_codes = detect_qr_codes(img)
        if self.display_fn:
            self.display_fn(img, detected_codes)
        
        for dc in detected_codes:
            qr_data = unpack_qr_data(dc)
            if self.result_handler_fn:
                self.result_handler_fn(qr_data)

    def full_loop(self):
        keep_running_forever = True
        while keep_running_forever:
            try:
                self.single_work_cycle()
            except Exception as ex:
                logger.error(f'exception received in the work-cycle. Log and ignore.\n{ex}')
                
    def run(self):
        self.full_loop()


if __name__ == '__main__':
    # parameters setup
    try:
        config = json.loads(Path('config.json').read_text())
    except OSError as e:
        logger.error(f"Error when trying to load config. Using default values ({Path('config.txt').absolute()})\n{e}")
        config = {}

    # config = {
    #     "camera_ip": "169.254.84.186",
    #     "axis_camera_config": {
    #         "username": "root",
    #         "password": "ButorRevolution"
    #     },
    #     "result_url": "https://localhost:44395/api/plans/{camera_ip_address}/{cfc_id}",
    #     "url_data_key": "cfc_id",
    #     "qr_data_key": "concreteFurnitureComponentId",
    #     "send_timeout_seconds": 5,
    #     "display_image": True,
    #     "use_image_preprocessing": False,
    #     "test_image": "C:/Users/pasztor.balazs/Pictures/QR/axis/4.bmp"
    # }
    
    camera_ip = config.get('camera_ip', '1.2.3.4')
    axis_camera_config = config.get('axis_camera_config', {'username': 'root', 'password': 'ButorRevolution'})
    axis_camera_config.update({'ip_address': camera_ip})
    result_url = config.get('result_url',
                            'https://localhost:44395/api/plans/{camera_ip_address}/{cfc_id}')
    url_data_key = config.get('url_data_key', 'cfc_id')
    qr_data_key = config.get('qr_data_key', 'concreteFurnitureComponentId')
    send_timeout_seconds = config.get('send_timeout_seconds', 5)
    display_image = config.get('display_image', False)
    use_image_preprocessing = config.get('use_image_preprocessing', False)
    
    def url_assembler_fn(qr_data):
        """ formats the url where the HTTP PUT request is sent
        :param qr_data: dict
        :return: str
        """
        return result_url.format(**{'camera_ip_address': camera_ip, url_data_key: qr_data[qr_data_key]})
    
    # initializing components
    if 'test_image' in config.keys():
        import matplotlib.pyplot as plt
        image_provider_fn = partial(plt.imread, config['test_image'])
    else:
        camera = AxisCamera.factory(axis_camera_config)
        image_provider_fn = partial(camera.get_jpg_image, compression=1)
    
    message_sender = MessageSender(url_assembler_fn=url_assembler_fn,
                                   timeout_seconds=send_timeout_seconds,
                                   qr_data_key=qr_data_key)
    
    controller = Controller(image_provider_fn=image_provider_fn,
                            display_fn=cv2_display if display_image else None,
                            image_preprocessor_fn=preprocess_image if use_image_preprocessing else None,
                            result_handler_fn=message_sender.send)
    # start
    controller.run()
