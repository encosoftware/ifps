from Parser.transformParameters import transform_components
from Sequence.converter import SequenceMaker, ComponentImage

from matplotlib import pyplot as plt


if __name__ == "__main__":
    json_path = "../Parser/json/A106-pl3.json"

    # add_sizes = [(100, 40, 0), (100, 0, 0), (0, 60, 0), (200, 0, 0), (0, 0, 10)]
    index = 6
    add_size = (100, 40, 0)
    # for add_size in add_sizes:
    for index in [0, 43, 44, 66, 6, 65, 10, 11]:
        trans, name = transform_components(json_path, index)
        hole = trans.transform_holes()
        poly = trans.transform_polygons()
        size = trans.get_size()
        seq = SequenceMaker(hole, poly, size)
        seqs = seq.make()
        # print(seqs)
        comp_img = ComponentImage(size.tolist())
        plt.figure(name + "_%s" % index, figsize=(6., 6.))
        plt.imshow(comp_img.draw(seqs))
        w_s = (size[0] + add_size[0])/size[0]
        h_s = (size[1] + add_size[1])/size[1]
        print((h_s, w_s))
        size2 = (size[0]+add_size[0], size[1]+add_size[1], size[2]+add_size[2])
        comp_img2 = ComponentImage(size2)
        plt.figure(name + "2_%s" % index, figsize=(6*h_s, 6*w_s))
        plt.imshow(comp_img2.draw(seqs))

        plt.show()
