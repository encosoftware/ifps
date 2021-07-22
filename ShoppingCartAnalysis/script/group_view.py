import json
from pathlib import Path
from matplotlib import pyplot as plt


path = Path('X:/ButorRevolution/Sandbox/TemplateExports')


with open(path/"groups.json") as jfile:
    data = json.load(jfile)

images = {}
for image in path.glob("*.png"):
    images[image.stem] = plt.imread(str(image))

index = 0
for key, values in data.items():
    index += 1
    if index < 5:
        continue
    for v in values:
        plt.figure(key + v)
        plt.imshow(images[v])
    plt.show()
