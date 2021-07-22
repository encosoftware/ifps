from pathlib import Path
import requests
import zipfile
from datetime import datetime


class TCNLine:
    def __init__(self):
        self._head = {}
        self._params = {}

    def __str__(self):
        return self.__repr__()

    def __repr__(self):
        s = "W%s{ %s "%(self._head['LC'], self._head['OC'])
        for key, value in self._head.items():
            if key in ['LC','OC']:
                continue
            s += "%s=%s " %(key, value)

        for key, value in self._params.items():
            s += "%s=%s " %(key, value)
        s += "}W"
        return s

    def parse_line(self, line):
        for part in line.split()[:-1]:
            if part[0:2] == 'W#':
                self._head['LC'] = part[1:-1]
            elif part[0:2] == '::':
                self._head['OC'] = part
            elif part[0] == '#':
                tmp = part.split('=')
                self._params[tmp[0]] = tmp[1]
            else:
                tmp = part.split('=')
                self._head[tmp[0]] = tmp[1]

    def add_all_to_head(self, datas):
        for key, value in zip(['LC', 'OC', 'MD'], datas):
            self._head[key] = value

    def set_param(self, key, value):
        self._params[key] = value

    def set_params(self, keys, values):
        for key, value in zip(keys, values):
            self._params[key] = value

    def have_key(self, key):
        return key in self._head.keys()

    def replace_key_value(self, key0, key1, value):
        tmp = {}
        for k, v in self._head.items():
            if k == key0:
                k = key1
                v = value
            tmp[k] = v
        self._head = tmp

    def get_line_code(self):
        return self._head['LC']

    def get_operation_code(self):
        return self._head['OC']

    def get_code(self, key):
        return self._head[key]

    def get_param(self, key):
        return self._params[key]

    def get_params(self, keys):
        return [self._params[key] for key in keys ]


class TCNProcessor:
    def __init__(self, path):
        self._dataDict = {}
        self.read_TCN(path)
        self._codeNumber = 1
        self._size = []
        self.read_size()

    def get_data(self):
        return self._dataDict

    def get_size(self):
        return self._size

    def is_back(self, t=4):
        return self._size[2] == t

    def is_panel(self, panels):
        for panel in panels:
            if panel.lower() in self._dataDict['H'][1].split(']')[0].lower():
                return True
        return False

    def read_TCN(self, path):
        data = path.read_bytes().decode('Latin-1').split('\r\n')[:-1]
        header = True
        attr = []
        key = 'H'
        for code in data:
            if code[-1] == '{':
                if header:
                    self._dataDict[key] = attr
                    attr = []
                    header = False
                key = code[:-1]
            elif code[0] == '}':
                self._dataDict[key] = attr
                attr = []
            elif code[0] == 'W':
                tcnLine = TCNLine()
                tcnLine.parse_line(code)
                attr.append(tcnLine)
            else:
                attr.append(code)

    def read_size(self):
        for line in self._dataDict['H']:
            if '::UN' in line:
                for number in line.split()[1:]:
                    self._size.append(float(number[3:]))
                break

    def form_sequence(self, lineCodes):
        st, ed = None, None
        for lC, ind in zip(lineCodes, range(len(lineCodes)) ):
            if type(lC) is not TCNLine:
                continue
            lC = lC.get_line_code()
            if st is None and lC == '#89':
                st = ind
            elif lC != '#2201' or ind + 1 == len(lineCodes):
                ed = ind + (0 if lC != '#2201' else 1)
                break
        return [st, ed]

    def circle_sequence(self, lineCodes):
        st, ed = None, None
        nextI, nextC = -1, '#89'
        for line, ind in zip(lineCodes, range(len(lineCodes)) ):
            if type(line) != TCNLine:
                continue
            lC = line.get_line_code()
            if st is None and lC == nextC:
                st = ind
                nextI =ind+1
                nextC = '#2201'
            elif nextI == ind and lC == nextC and nextC == '#2201':
                nextI += 1
                nextC = '#2101'
            elif nextI == ind and lC in [nextC, '#2101']:
                nextI += 1
                if nextC != 'HAVE':
                    nextC = 'HAVE'
            elif nextI == ind and lC == '#2201' and nextC == 'HAVE':
                ed = ind + 1
                break
            else:
                nextC = '#89'
                st, ed = None, None
        return [st, ed]

    def sequence_indexes(self, key, func):
        lineCodes = [line for line in self._dataDict[key]]
        indexList = []
        while True:
            st, ed = func(lineCodes)
            if st is not None:
                indexList.append([st, ed])
                lineCodes = lineCodes[ed:]
            else:
                break
        return indexList

    def sequence_indexes_(self, key, func):
        if 'SIDE' not in key:
            return []
        lines = self._dataDict[key]
        secq = False
        ind = [0,0]
        indList = []
        len_ = len(lines)
        for line, lineNum in zip(lines, range(len_)):
            if type(line) != TCNLine:
                continue
            lC = line.get_line_code()
            ret = func(secq, lC, lineNum, len_)
            if type(ret) == tuple:
                secq, rI, rV = ret
                ind[rI] = rV
            elif not ret:
                ind = [0,0]
        return indList

    def remove_offset(self):
        self._dataDict['OFFS'][:2] = ['#0=0|0','#1=0|0']

    def remove_form(self):
        self.remove_offset()
        for key, lines in self._dataDict.items():
            if 'SIDE' not in key:
                continue

            indL = self.sequence_indexes(key, self.form_sequence)
            for inds in indL:
                if self.check_form(key, inds):
                    self._dataDict[key] = lines[:inds[0]] + lines[inds[1]:]
                    break

    def check_form(self, key, indexes):
        prev = []
        for line in self._dataDict[key][indexes[0]:indexes[1]]:
            pos = []
            coordinate = []
            for coord, bound in zip(line.get_params(['#1', '#2']), self._size[:2]):
                coord = float(coord)
                if coord < 0 or coord > bound:
                    control = +1
                elif 0 < coord < bound:
                    control = -1
                else:
                    control = 0
                pos.append(control)
                coordinate.append(coord)
            if sum(pos) == -2:
                return False

            if len(prev) != 0 and abs(prev[0] - coordinate[0]) >= 10 and abs(prev[1] - coordinate[1]) >= 10:
                return False
            prev = coordinate
        return True

    def circle_transform(self, key):
        indexes = self.sequence_indexes(key, self.circle_sequence)
        for ind in indexes:
            if self._dataDict[key][ind[0]+2].get_operation_code() != '::WTa':
                continue
            rev = int(self._dataDict[key][ind[0]+2].get_param('#34')) == 1
            self.correct_circle_milling(key, ind, rev)

    def correct_circle_milling(self, key, indexes, reverse):
        allLinePos = []
        posKey = ['#1', '#2', '#3']
        realZ = None
        _range = range(indexes[1]-1,indexes[0]-1, -1) if reverse else range(indexes[0],indexes[1])
        for line, ind in zip( self._dataDict[key][indexes[0]:indexes[1]], _range):
            coord = line.get_params(posKey)
            if realZ is None and line.get_operation_code() == '::WTa':
                realZ = coord[2]
            allLinePos.append( (ind, coord) )
        for ind, values in allLinePos:
            if ind in [indexes[0]+1, indexes[1]-2]:
                values[2] = realZ
            self._dataDict[key][ind].set_params(posKey, values)
            if reverse and self._dataDict[key][ind].get_operation_code() == "::WTa":
                self._dataDict[key][ind].set_param('#34', 0)

    def renumbering(self, key):
        for line, ind in zip(self._dataDict[key], range(len(self._dataDict[key])) ):
            if type(line) == TCNLine:
                k = "%s" %('WF' if line.have_key('WF') else ('WS' if line.have_key('WS') else ''))
                if len(k) == 0:
                    continue
                self._dataDict[key][ind].replace_key_value(k, 'WS', self._codeNumber)
                self._codeNumber += 1

    def transform(self):
        transfData = {}
        sides = []
        for key in self._dataDict.keys():
            if 'SIDE' in key:
                if 1 < len(self._dataDict[key]):
                    self.renumbering( key)
                    self.circle_transform(key)
                    sides.append(int(key.split('#')[-1]))
        for key, value in self._dataDict.items():
            if key == 'H':
                value[2] = '::SIDE='
                for side in sides:
                    value[2] += '%s;' %side
                if len(value) > 4:
                    value = value[:-2]
            elif key in ['EXE', 'INFO']:
                continue
            elif key == 'OFFS':
                for item in ['#3=0|0', '#4=0|0', '#5=0|0', '#6=0|0', '#7=0|0']:
                    if item not in value:
                        value.append(item)
            elif key == 'SIDE#0':
                key = 'PREV'
                value = []
            transfData[key] = value
        self._dataDict = transfData

    def dump_TCN(self, path):
        outStr = ''
        for key, lines in self._dataDict.items():
            if key != 'H':
                outStr += key + '{\r\n'
            for line in lines:
                outStr += "%s\r\n" %line
            if key != 'H':
                if 'SIDE' in key:
                    key = 'SIDE'
                outStr += '}' + key + '\r\n'

        path.write_bytes(outStr.encode('Latin-1'))


def processing_TCN(inPath, outPath, panels):
    outDirFront = Path(outPath) / "front/"
    outDirOther = Path(outPath) / "other/"
    outDirFront.mkdir(parents=True, exist_ok=True)
    outDirOther.mkdir(parents=True, exist_ok=True)

    panelCounter = {'FRONT': 0, 'BACK':0, 'OTHER':0}

    inPath = Path(inPath)
    if not inPath.exists():
        raise FileNotFoundError('No such directory as %s!' %inPath)
    empty = len(panels[0]) == 0
    for filePath in inPath.glob('**/*.tcn'):
        processor = TCNProcessor(filePath)

        back = processor.is_back()
        if back:
            panelCounter['BACK'] += 1
            continue

        panel = processor.is_panel(panels) if not empty else False
        processor.transform()
        if panel:

            processor.remove_form()
            path = outDirFront
            panelCounter['FRONT'] += 1
        else:
            path = outDirOther
            panelCounter['OTHER'] += 1

        outFileAbsol = path / filePath.name
        processor.dump_TCN(outFileAbsol)

    return panelCounter


def send_data(API, inPath, report):
    if API is None:
        return 'No API'

    dirPath = Path(inPath)
    paths = []

    for path in dirPath.glob('**/*'):
        paths.append( (path, path.relative_to(dirPath)) )
    zipPath = dirPath/"tcn.zip"
    zipFile = zipfile.ZipFile(zipPath, 'w', zipfile.ZIP_BZIP2)

    for path, relativePath in paths:
        zipFile.write(path, relativePath)
    zipFile.close()

    zipAPI = open(zipPath, 'rb')
    ret = False
    try:
        req = requests.post(API,report, files={'archive': ('tcn.zip', zipAPI)})
        ret = req.status_code == 200
    except requests.exceptions.MissingSchema as e:
        report['EXCEPTION'] = e
        ret = 'Exception'

    zipPath.unlink()
    return ret


def log_TCN(success, report):
    logPath = Path.cwd()/'log.txt'
    s = '%s :: Success: %s -- Report: %s\r\n' %(datetime.now(), success, report)
    log = open(logPath, 'a')
    log.write(s)
    log.close()


"""
Good <--
G21
G90
G00 X296 Y132 Z0
G01 X283 Y129 Z-17
G02 X202 Y209 Z-17 R80
G02 X362 Y209 Z-17 R80
G02 X280 Y129 Z-17 R80
G01 X267 Y132 Z0

W#89{ ::WTs WS=13 WN=2  #8015=0 #1=296.2797 #2=132.3182 #3=0.00 #201=1 #203=1 #205=2222 #1001=100 #9521=0 #8101=0 #8096=0 #40=2 #36=0 #46=1 #8135=0 #8136=0 #9511=0 #9512=0 }W
W#2201{ ::WTl  #8015=0 #1=283.4999 #2=129.0141 #3=-17 #42=0 #2008=2 }W
W#2101{ ::WTa  #8015=0 #1=202 #2=209 #3=-17.00 #34=0 #31=a;282 #32=a;209 #42=0 }W
W#2101{ ::WTa  #1=362 #2=209 #3=-17.00 #34=0 #31=a;282 #32=a;209 #42=0 }W
W#2101{ ::WTa  #1=280.5001 #2=129.0141 #3=-17 #34=0 #31=a;282 #32=a;209 #42=0 }W
W#2201{ ::WTl  #8015=0 #1=267.7203 #2=132.3182 #3=0 #42=0 }W

Bad <--
G21
G90
G00 X267 Y132 Z0
G01 X280 Y129 Z0
G03 X362 Y209 Z-17 R80
G03 X202 Y209 Z-17 R80
G03 X283 Y129 Z0 R80
G01 X296 Y132 Z0

W#89{ ::WTs WS=1 WB=0 WN=2 #8015=0 #1=267.7203 #2=132.3182 #3=0 #201=1 #203=1 #205=2222 #1001=100 #8101=0 #8096=0 #36=0 #40=1 #43=0 #46=1 #8135=0 #8136=0 #8180=0 #8181=0 #8185=0 #8186=0 #9511=0 #9512=0 #9513=0 }W
W#2201{ ::WTl #1=280.5001 #8054=0 #2=129.0141 #8055=0 #3=0.00 #8056=0 #8015=0 #9022=0 }W
W#2101{ ::WTa #1=362 #2=209 #3=-17.00 #34=1 #31=a;282 #32=a;209 }W
W#2101{ ::WTa #1=202 #2=209 #3=-17.00 #34=1 #31=a;282 #32=a;209 }W
W#2101{ ::WTa #1=283.4999 #2=129.0141 #3=0.00 #34=1 #31=a;282 #32=a;209 }W
W#2201{ ::WTl #1=296.2797 #8054=0 #2=132.3182 #8055=0 #3=0.00 #8056=0 #8015=0 #9022=0 }W"""


