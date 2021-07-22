import time
from typing import Dict, Callable
import requests
import logging
from  expiringdict import ExpiringDict

logging.basicConfig(format='%(asctime)s.%(msecs)03d %(levelname)-6s:: %(message)s',
                    datefmt='%Y-%m-%d %H:%M:%S')
logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)


class MessageSender:
    def __init__(self, url_assembler_fn: Callable[[dict], str], timeout_seconds=5,
                 qr_data_key='concreteFurnitureComponentId'):
        self.url_assembler = url_assembler_fn
        self.timeout_seconds = timeout_seconds
        self.message_cache = ExpiringDict(max_len=1000, max_age_seconds=self.timeout_seconds)
        self.qr_data_key = qr_data_key
        
    def send(self, qr_data: dict):
        cache_key = qr_data[self.qr_data_key]
        if cache_key not in self.message_cache.keys():
            timestamp = time.ctime(time.monotonic())
            success = _send_result(qr_data, self.url_assembler)
            if success:
                self.message_cache[cache_key] = timestamp
                logger.debug(f"successfully sent message at {timestamp}, component added to timeout.")
    

def _send_result(qr_data: Dict[str, str], url_assembler: Callable[[dict], str]) -> bool:
    url = url_assembler(qr_data)
    try:
        logger.debug(f'HTTP put to {url}')
        resp = requests.put(url, verify=False)
        if resp.status_code == 200:
            return True
    except Exception as e:
        logger.debug(e)
    return False
