from DataHandler.ApiClient import ApiClientClass, furniture_unite_price, furniture_unite_size, convert_xxl,\
    parse_xxl, appliance_data
from pathlib import Path
import pandas
import json
import logging
import sys

logger = logging.getLogger(__name__)
logger.setLevel(logging.INFO)

handler = logging.StreamHandler(sys.stdout)
handler.setFormatter(logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s'))
logger.addHandler(handler)


class Upload:
    def __init__(self, api_client: ApiClientClass, seed_path: Path):
        self.__client = api_client
        self.__seed_path = seed_path

    def all_furniture_unit(self):
        logger.info(f'Uploading furniture units!')
        log_count = 0

        all_size = furniture_unite_size(self.__seed_path / "sizes.txt")
        for f_unit_path in self.__seed_path.iterdir():
            if not (f_unit_path / (f_unit_path.name + ".csv")).exists():
                continue
            df = pandas.read_csv(f_unit_path / (f_unit_path.name + ".csv"), sep=";")
            df.to_csv(self.__seed_path / (f_unit_path.name + ".csv"), sep=";", index=False)

            f_unit = {'containerName': str(self.__seed_path)+"\\", 'fileName': (f_unit_path.name + ".csv")}
            price = furniture_unite_price(f_unit_path / "price.csv")
            key = f_unit_path.name.lower()
            f_unit_image = f_unit_path / (f_unit_path.name + ".png")
            res = self.__client.post_image(image_param={'container': 'FurniturePictures'},
                                           image_file={'file': open(str(f_unit_image), 'rb')})
            dto = {
                "price": {
                    "value": round(price * 1.15, 2),
                    "currencyId": 2
                },
                "materialCost": {
                    "value": price,
                    "currencyId": 2
                },
                "image": {
                    "containerName": res['item1'],
                    "fileName": res['item2']
                },
                "width": all_size[key]['width'],
                "height": all_size[key]['height'],
                "depth": all_size[key]['depth']
            }
            self.__client.post_furniture_unit(json.loads(json.dumps(dto)), f_unit)
            log_count += 1
        logger.info(f'{log_count} furniture units uploaded!')

    def all_sequences(self, seq_maker):
        logger.info(f'Uploading sequences!')
        log_count = 0

        checked = []
        for unit_id in self.__client.get_furniture_unit():
            if not (self.__seed_path / unit_id['name']).exists() or unit_id['name'] in checked:
                continue
            checked.append(unit_id['name'])
            logger.info(f"Uploading sequences for {unit_id['name']} furniture unit!")

            names = ["%s_%s" % (i, n) for i, n in enumerate(pandas.read_csv(
                self.__seed_path / unit_id['name'] / (unit_id['name'] + ".csv"), sep=";")['Board Name'].dropna())]
            for component in self.__client.get_furniture_component_by_unit_id(unit_id['id']):
                for index, name in enumerate(names):
                    if component['name'] in name:
                        names.pop(index)
                        paths = list((self.__seed_path / unit_id['name'] / 'cnc').glob("%s*" % name))
                        if len(paths) == 0:
                            continue

                        if len(paths) == 1:
                            paths = paths[0]
                        seqs = seq_maker(*convert_xxl(parse_xxl(paths))).make()

                        for s in seqs:
                            seq_json = json.loads(json.dumps(s.to_dict()))
                            if s.is_rectangle():
                                self.__client.post_rectangle({'componentId': component['id']}, seq_json)
                            elif s.is_drill():
                                self.__client.post_drill({'componentId': component['id']}, seq_json)
                            log_count += 1
                        break
        logger.info(f"{log_count} sequences uploaded!")

    def all_appliance(self):
        logger.info(f"Uploading appliances!")

        for appliance_json in appliance_data(self.__seed_path):
            self.__client.post_appliance(json.loads(json.dumps(appliance_json)))

    def materials(self):
        logger.info(f"Uploading materials!")

        self.__client.post_material(
            {
                'container': str(self.__seed_path) + "\\",
                'fileName': "materials2.csv"
            })

    def material_images(self):
        logger.info(f"Uploading material images!")

        ret = {}
        for img_path in (self.__seed_path / "mat").glob("*.jpg"):
            res = self.__client.post_image(image_param={'container': 'MaterialPictures'},
                                           image_file={'file': open(str(img_path), 'rb')})
            ret[img_path.stem] = res['item1'], res['item2']
        return ret
