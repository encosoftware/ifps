import time
import numpy as np
import cv2


def dbg_draw_bbox_to_img(img, obj):
    corners = np.array([tuple(pp) for pp in obj.polygon])
    img = cv2.drawContours(img, [corners], -1, (0, 255, 0), 3)
    center = np.rint(np.average(corners, axis=0)).astype(np.int)
    font = cv2.FONT_HERSHEY_SIMPLEX
    text = obj.data.decode(encoding='UTF-8')
    print(text)
    cv2.putText(img, text, tuple(center), font, .5, (0, 0, 255), 2, cv2.LINE_AA)
    return img


def cv2_display(orig, objects):
    img = np.array(orig)
    for o in objects:
        img = dbg_draw_bbox_to_img(img, o)
    cv2.imshow('QR', cv2.cvtColor(img, cv2.COLOR_RGB2BGR))
    cv2.waitKey(1)
