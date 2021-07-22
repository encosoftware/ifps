import io
import json


def parseTop(inputString):
    ioString = io.StringIO(inputString)
    prevUnitId = 1

    data = {
        'units' : []
    }
    unit = {
        'components' : [],
        'pos' : 1
    }

    for line in ioString:
        # strip whitespaces and starting and closing {}
        line = line.strip()
        line = line[1:-1]
        tokens = line.split('}{')
        
        for t in tokens:
            t = t.strip()
            print(t, end=' | ')

        print('\n')

        tmpParts = tokens[2].split('.')
        unitPos = int(tmpParts[0])
        # create new unit and append the old one
        if (unitPos != prevUnitId):
            data['units'].append(unit)
            unit = {
                'components' : [],
                'pos' : unitPos
            }

        # create component for every line
        component = {}
        compPos = float(tmpParts[1])
        component['unitPos'] = unitPos
        component['compPos'] = compPos

        length = float(tokens[4])
        component['length'] = length
        width = float(tokens[5])
        component['width'] = width
        height = float(tokens[6])
        component['height'] = height

        tmpParts = tokens[27].split('=')
        mainMaterial = tmpParts[1].strip()
        component['mainMat'] = mainMaterial

        tmpParts = tokens[30].split('=')
        edgeMaterial1 = tmpParts[1].strip()
        component['eMat1'] = edgeMaterial1
        tmpParts = tokens[31].split('=')
        edgeMaterial2 = tmpParts[1].strip()
        component['eMat2'] = edgeMaterial2
        tmpParts = tokens[32].split('=')
        edgeMaterial3 = tmpParts[1].strip()
        component['eMat3'] = edgeMaterial3
        tmpParts = tokens[33].split('=')
        edgeMaterial4 = tmpParts[1].strip()
        component['eMat4'] = edgeMaterial4

        tmpParts = tokens[35].split('=')
        edgeSize1 = float(tmpParts[1])
        component['eSize1'] = edgeSize1
        tmpParts = tokens[36].split('=')
        edgeSize2 = float(tmpParts[1])
        component['eSize2'] = edgeSize2
        tmpParts = tokens[37].split('=')
        edgeSize3 = float(tmpParts[1])
        component['eSize3'] = edgeSize3
        tmpParts = tokens[38].split('=')
        edgeSize4 = float(tmpParts[1])
        component['eSize4'] = edgeSize4

        tmpParts = tokens[46].split('=')
        amount = int(tmpParts[1])
        component['amount'] = amount

        tmpParts = tokens[47].split('=')
        name = tmpParts[1]
        component['name'] = name
        
        prevUnitId = unitPos
        
        # escape first rows per unit - useless info
        if(edgeMaterial1 == 'BU' and edgeSize1 == 12 and compPos == 0):
            continue
        else:
            # print('%d-%d \t%d,  %d  %d \tN: %d \t%s \t\t Materials: %s | %s %3.f | %s %2.f | %s %2.f | %s %2.f' % (unitPos, compPos, length, width, height, amount, name, mainMaterial, edgeMaterial1, edgeSize1, edgeMaterial2, edgeSize2, edgeMaterial3, edgeSize3, edgeMaterial4, edgeSize4))
            unit['components'].append(component)

    data['units'].append(unit)

    return data    


def main():
    filePath = './Parser/data/A106.top'

    fileHandle = open(filePath, 'r')
    str = fileHandle.read()
    
    data = parseTop(str)

    with open('./Parser/json/A106-top.json', 'w') as outFile:
        json.dump(data, outFile)


if __name__ == "__main__":
    main()