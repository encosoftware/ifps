from random import Random
import numpy as np


def age_distribution(age_min, age_max):
    x = ((np.arange(age_min, age_max+1) - age_min) / (age_max - age_min)) * 6 - 3
    y = 1 / (1 + np.exp(x))
    return {k: v for k, v in zip(range(age_min, age_max+1), y)}


def days_random_distribution(peak_min, peak_max, init=0.3, sign=1):
    act = init
    i_min = 0
    i_max = 0
    ret = {}
    rnd = Random()
    if sign == 0:
        sign = 1
    for i in range(1, 32):
        act += round(rnd.random(), 1) * sign
        if act >= peak_max[i_max] and sign > 0:
            sign = -1
            act = peak_max[i_max]
            i_max += 1 if i_max + 1 < len(peak_max) else - i_max
        elif act <= peak_min[i_min] and sign < 0:
            sign = 1
            act = peak_min[i_min]
            i_min += 1 if i_min + 1 < len(peak_min) else - i_min
        ret[i] = act
    s = sum(ret.values())
    return {k: round(v/s, 3) for k, v in ret.items()}
