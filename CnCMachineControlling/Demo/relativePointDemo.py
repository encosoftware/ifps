import numpy as np

A = np.array( [500,200,20] )
B = np.array( [520,200,20] )


print("(A) solution")

p1 = np.array( [250,100,10] )
p2 = np.array( [10, 10, 10] )
p3 = np.array( [50, 10, 10] )
p4 = np.array( [150, 30, 0] )
p5 = np.array( [490, 190, 19] )

ip1 = np.array([260,100,10])
ip2 = np.array([10,10,10])
ip3 = np.array([50,10,10])
ip4 = np.array([160,30,10])
ip5 = np.array([510,190,19])

somt = (B - A)/A
isomt = (A-B)/B

for p, ip in [(p1, ip1),(p2, ip2),(p3, ip3),(p4, ip4),(p5, ip5)]:
    print( "%s -> %s ::: %s -> %s" %(p, p*(1+somt), ip, ip*(1+isomt) ) )

print("\r\n\r\n(B) solution")


class Points:
    def __init__(self, size):
        self._dict = {'C': size/2, 'TopSW': np.array([0,0,0]), 'DownNE': size }
        self._dict['TopNW'] = np.array([0, size[1],0])
        self._dict['TopNE'] = np.array([size[0], size[1], 0])
        self._dict['TopSE'] = np.array([size[0], 0, 0])
        self._dict['DownSE'] = np.array([size[0],0,size[2]])
        self._dict['DownSW'] = np.array([0,0,size[2]])
        self._dict['DownNW'] = np.array([0, size[1], size[2]])

    def getRelative(self, point):
        key = 'TopNE'
        dist = np.inf
        p = 0
        for k, v in self._dict.items():
            d = np.linalg.norm(v-point)
            if d  < dist:
                dist = d
                key = k
                p = point-v
        return (key, p)

    def getAbs(self, key, p):
        return self._dict[key] + p


pA = Points(A)
pB = Points(B)

for p, ip in [(p1, ip1),(p2, ip2),(p3, ip3),(p4, ip4),(p5, ip5)]:
    k, tp = pA.getRelative(p)
    ik, itp = pB.getRelative(ip)
    print( "%s -> %s ::: %s -> %s" %(p, pB.getAbs(k, tp), ip, pA.getAbs(k, itp) ))

print("\r\n________(B) solution is the possible option!________")