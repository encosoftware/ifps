from pathlib import Path
import pandas as pd
import json
import sys
from os import path
sys.path.append(path.abspath('..'))
sys.path.append(path.abspath('..\\..\\ScheduleOptimisation'))

from DataGenerator.SeedUploader import Upload
from DataHandler.ApiClient import ApiClientClass, parse_xxl, convert_xxl

from Sequence.converter import SequenceMaker


def collect_component_csv(root_dir):
    ret = []
    for dir_ in root_dir.glob('**/'):
        if dir_.parent.name != root_dir.name:
            continue
        for csv in dir_.glob("%s.csv" % dir_.name):
            ret.append(csv)
    return ret


def func_material(data):
    ret_d = {}
    for c, d in zip(data['Material Code'], data['Fiber']):
        ret_d[c] = bool(1 - d)
    return ret_d


def func_material_to_df(data, pic_names=None):
    if pic_names is None:
        file_names = [name + '.jpg' for name in data.keys()]
    else:
        file_names = [name for _, name in pic_names.values()]
    le = len(data)
    return pd.DataFrame(list(zip(data.keys(), [18]*le, data.values(), ['MaterialPictures']*le, file_names)),
                        columns=['Material Code', 'Width', 'Has Fiber Direction', 'container', 'file name'])


def make_material2_csv_from_upload(root_path, image_data):
    df = pd.read_csv(root_path / "materials.csv", sep=";")
    df2 = func_material_to_df(func_material(df), image_data)
    df2.to_csv(root_path / "materials2.csv", sep=";", index=False)


def collect_material_image_path(data, root_dir):
    ret = []
    for code in data["Material Code"]:
        for p in root_dir.glob("**/%s.*" % code):
            ret.append(p)
    return ret


def the_do_not_use_script():
    import shutil
    for unit_dir in Path("C:/Users/vamos.peter/Projects/Butor/Documentation/Documents/seed/").iterdir():
        if not unit_dir.is_dir() or not (unit_dir / ("%s.csv" % unit_dir.name)).exists():
            continue
        print(unit_dir.name)

        unit = pd.read_csv(
            unit_dir / (unit_dir.name + ".csv"), sep=";"
        )[['Element Name', 'Board Name', 'Width', 'Length', 'Thickness']].dropna()
        code_size = {(dir_n, name): {'code': dir_n[0] + name[0], 'size': sorted([l, w, t], reverse=True)}
                     for dir_n, name, w, l, t in unit.values}
        board_data = {k: [] for k in unit['Board Name'].unique()}
        file_names = {k: 0 for k in unit_dir.glob("**/*.xxl")}
        for xxl_file in file_names.keys():
            act_dir = xxl_file.parent.name
            act_code = xxl_file.name[2:4]
            hole, rect, size = convert_xxl(parse_xxl(xxl_file))
            for k, v in code_size.items():
                if k[0].replace(' ', '_') != act_dir or v['code'] != act_code or v['size'] != size:
                    continue
                board_data[k[1]].append(xxl_file)

        for index, val in enumerate(unit[['Board Name', "Thickness"]].values):
            if val[1] == 3:
                continue
            key = val[0]

            tmp = [(k, file_names[k]) for k in board_data[key]]
            if len(tmp) == 0:
                continue
            file_name, _ = sorted(tmp, key=lambda x: x[1])[0]

            to_path = unit_dir / "cnc"
            if not to_path.exists():
                to_path.mkdir()
            if file_name.name[0] == "2":
                for ind, f in enumerate(file_name.parent.glob("**/*%s.xxl" % file_name.stem[2:])):
                    file_names[f] += 1

                    shutil.copy(f, to_path / ("%s_%s_%s.xxl" % (index, key, ind)))
                # break
            else:
                file_names[file_name] += 1
                if not to_path.exists():
                    to_path.mkdir()
                shutil.copy(file_name, to_path / ("%s_%s.xxl" % (index, key)))
                # break


def upload_seed(api, seed_path, login_json):
    # Upload init
    upload = Upload(api, seed_path)

    # Login
    api.login(login_json)

    # Upload material images
    material_data = pd.read_csv(seed_path / "materials.csv", sep=";")
    collect_material_image_path(material_data, seed_path)
    image_data = upload.material_images()

    # Upload material data
    make_material2_csv_from_upload(seed_path, image_data)
    upload.materials()

    # Upload furniture unit
    upload.all_furniture_unit()

    # Upload sequences
    upload.all_sequences(SequenceMaker)

    # Upload appliances
    upload.all_appliance()

    # Logout
    api_client.logout()


if __name__ == "__main__":
    api_client = ApiClientClass(Path("../data/api_config.json"))
    login_data = json.loads(json.dumps({"email": "enco@enco.hu", "password": "password", "rememberMe": True}))

    # ------------------------- Upload seed data -------------------------
    upload_seed(api_client, Path("C:/Users/vamos.peter/Projects/Butor/Documentation/Documents/seed/"), login_data)
