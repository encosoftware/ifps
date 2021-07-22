import cv2
import matplotlib.pyplot as plt

RootPath = '/home/vamos.peter/Project/Butor/data/'

def getPic(path):
    A = cv2.imread(path,-1)
    if A is None:
        print("THIS IS NONE!")
    else:
    	plt.imshow(A)
    	plt.show()


def getBinary(path):
	f = open(path, 'rb')
	data = f.readlines()
	f.close()

	ind = 0
	print(len(data))
	#char = list()
	f = open(RootPath + 'test.txt', 'w')
	#b = open(RootPath + 'mytest.pgm', 'wb')
	for dd in data[0:]:
		temp = dd.decode('Latin-1', 'strict')
		if ind == 0:
			ind += 1
			temp = temp.split('Xilog')
			#b.write( (temp[0] + 'Xilog').encode('Latin-1')  )
			temp = temp[1]
		sptemp = temp.split('V=800')
		strtemp = sptemp[0] + 'V=2000'
		for tt in sptemp[1:]:
			strtemp += tt
		#b.write( strtemp.encode('Latin-1'))
		t = ''
		for ch in zip(dd[-len(temp):] ,temp):
			f.write(str(ch)+'\r\n')
	f.close()
	#b.close()


def tryPGM(path, v=1000):
	f = open(path, 'rb')
	data = f.readlines()
	f.close()
	ins = 'V=' +str(v)
	c0, c1, c2, c3 = (0,0,0,0)
	c0 = v%256
	c1 = int(v//256)
	c2 = int(c1 // 256)
	c3 = int(c2 // 256)
	c2 %= 256
	c1 %= 256
	print("%s -> %s" %(ins,(c0, c1, c2, c3)) )
	ind = 0
	print(len(data))
	b = open(RootPath + 'mytest.pgm', 'wb')
	for dd in data[0:]:
		temp = dd.decode('Latin-1', 'strict')
		off = 0
		if ind == 0:
			ind += 1
			temp = temp.split('Xilog')
			off = len(temp[0])
			b.write( (temp[0] + 'Xilog').encode('Latin-1')  )
			temp = temp[1]
		sptemp = temp.split('V=800')
		strtemp = sptemp[0][:-29] + chr(c0) + chr(c1)+ chr(c2)+ chr(c3) +  sptemp[0][-25] + chr(27) + sptemp[0][-23:]  + 'V=800'# ins
		for tt in sptemp[1:]:
			strtemp += tt
		b.write( strtemp.encode('Latin-1'))
	b.close()

def mainPGM():
	path0 = RootPath + 'DOBOZ_VÉGZÁRÓ.PGM'
	path1 = RootPath + 'test.pgm'
	path2 = RootPath + 'mytest.pgm'
	#tryPGM(path0)
	#getPic(path0)

	getBinary(path1)
	#getPic(path1)
	#tryPGM(path1, 900)

	#getBinary(path2)
	"""In one line the image AND THE HEADER OF THE PROGRAM!!!"""

if __name__=="__main__":
	mainPGM()
