import json
from typing import List
import numpy as np
from pyzbar import pyzbar
from pyzbar.wrapper import ZBarSymbol
import logging
import cv2


logging.basicConfig(format='%(asctime)s.%(msecs)03d %(levelname)-6s:: %(message)s',
                    datefmt='%Y-%m-%d %H:%M:%S')
logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)
# logger.setLevel(logging.INFO)


def check_input_img(img):
    if not isinstance(img, np.ndarray):
        logger.error(f'incorrect type for image: {type(img)}, expecting np.ndarray')
        return False
    if img.dtype != np.uint8:
        logger.error(
            f'incorrect image numpy dtype: {img.dtype}, expecting np.uint8. Possible cause is png image source.')
        return False
    return True


def detect_qr_codes(img: np.ndarray) -> List[pyzbar.Decoded]:
    return pyzbar.decode(img, symbols=[ZBarSymbol.QRCODE])


def unpack_qr_data(detected_code: pyzbar.Decoded):
    det = fix_json___temporary(detected_code)
    d = json.loads(det)
    return d


def fix_json___temporary(detected_code):
    det = detected_code.data
    det = det.decode(encoding='UTF-8')
    det = det.replace('concreteFurnitureComponentId', '"concreteFurnitureComponentId"')
    det = det.replace('orderId', '"orderId"')
    return det


def preprocess_image(orig):
    return cv2.edgePreservingFilter(orig, sigma_s=200, sigma_r=0.005)
