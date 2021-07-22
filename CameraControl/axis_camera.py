from typing import Optional
import requests
from PIL import Image as PilImage
from io import BytesIO
import numpy as np
import xml.etree.ElementTree as ET


class AxisCamera:
    # originally written for AXIS P1405-LE Mk II Network Camera
    def __init__(self, ip_address, username, password):
        self.ip_address = ip_address
        self.auth = requests.auth.HTTPDigestAuth(username, password)
    
    def _send_request(self, request_string):
        # Let all exceptions raised for this time...
        response = requests.get(f'http://{self.ip_address}/{request_string}', auth=self.auth)
        if response.status_code != 200:
            try:
                msg = ET.fromstring(response.text).find('HEAD').getchildren()[0].text
            except (ET.ParseError, IndexError):
                msg = f'AxisCamera: request send - the returned status code is: {response.status_code}'
            raise OSError(msg)
        return response

    @staticmethod
    def _request_to_image_obj(request) -> np.ndarray:
        pil_image = PilImage.open(BytesIO(request.content))
        img = convert_pil_to_ndarray(pil_image)
        return img

    def get_jpg_image(self, compression: Optional[int] = None, resolution=None, color=None) -> np.ndarray:
        params = [
            f'resolution={resolution[0]}x{resolution[1]}' if resolution is not None else None,
            f'compression={compression}' if compression is not None else None,  # compression=0 results the best image.
            f'color={int(color)}' if color is not None else None
        ]
        query = f'jpg/image.jpg?{"&".join([v for v in params if v is not None])}'
        return self._request_to_image_obj(self._send_request(query))

    def get_bmp_image(self) -> np.ndarray:
        return self._request_to_image_obj(self._send_request('bitmap/image.bmp'))

    def get_supported_image_resolutions(self):
        response, _ = self._send_request('/axis-cgi/admin/param.cgi?action=list&group=Properties.Image.Resolution')
        return response.text.split('\n')[0].split('=')[1].split(',')

    def get_supported_file_types(self):
        response, _ = self._send_request('/axis-cgi/admin/param.cgi?action=list&group=Properties.Image.Format')
        return response.text.split('\n')[0].split('=')[1].split(',')

    @staticmethod
    def factory(cfg) -> "AxisCamera":
        params = {k: cfg[k] for k in cfg.keys() & {'ip_address', 'username', 'password'}}
        return AxisCamera(**params)


def convert_pil_to_ndarray(pil_image: PilImage) -> Optional[np.ndarray]:
    # Conversion often fails so this is hack to ensure a correct result
    for _ in range(5):
        img = np.array(pil_image)
        if len(img.shape) >= 2:
            return img
    else:
        return None


