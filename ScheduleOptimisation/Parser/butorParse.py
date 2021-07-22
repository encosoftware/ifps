import json
import os 
import io
import sys


def parse(inputString):
        sections = ['[PROJECT]', '[GROUP]', '[MAINPART]', '[SUBPART]', '[HOLE]', '[SUBBODY]']

        # init dicts
        data = {
                'units' : []
        }
        tmpUnit = {
                'components' : []
        }
        tmpComponent = {
                'holes' : [],
                'subbodies' : []
        }
        tmpHole = {}
        tmpSubbody = {}

        sectionId = -1

        ioString = io.StringIO(inputString)
        for line in ioString:
                line = line.strip()

                # skip empty lines and comments
                if line == '' or line[0] == ';':
                        pass
                
                # check for section headers
                elif line in sections:
                        sectionId = sections.index(line)

                        if sectionId == 1:
                                if 'Groupindex' in tmpUnit:
                                        # at group closing you have to add the unclosed holes and subbodies to the part and the part to the group
                                        if tmpHole:
                                                tmpComponent['holes'].append(tmpHole)
                                        if tmpSubbody:
                                                tmpComponent['subbodies'].append(tmpSubbody)
                                        tmpHole = {}
                                        tmpSubbody = {}

                                        tmpUnit['components'].append(tmpComponent)
                                        tmpComponent = {
                                                'holes' : [],
                                                'subbodies' : []
                                        }

                                        data['units'].append(tmpUnit)
                                        tmpUnit = {
                                                'components' : []
                                        }

                        if sectionId == 2 or sectionId == 3:
                                if 'PartIndex' in tmpComponent:
                                        # at part closing you have to add the unclosed holes and subbodies to the part
                                        if tmpHole:
                                                tmpComponent['holes'].append(tmpHole)
                                        if tmpSubbody:
                                                tmpComponent['subbodies'].append(tmpSubbody)
                                        tmpHole = {}
                                        tmpSubbody = {}
                                        
                                        tmpUnit['components'].append(tmpComponent)
                                        tmpComponent = {
                                                'holes' : [],
                                                'subbodies' : []
                                        }

                        if sectionId == 4:
                                if 'PartIndex' in tmpHole:
                                        if tmpHole:
                                                tmpComponent['holes'].append(tmpHole)
                                        tmpHole = {}

                        if sectionId == 5:
                                if 'PartIndex' in tmpSubbody:
                                        if tmpSubbody:
                                                tmpComponent['subbodies'].append(tmpSubbody)
                                        tmpSubbody = {}

                # key value pairs
                else:
                        parts = line.split('=', 1)
                        key = parts[0].strip()
                        value = parts[1].strip()

                        # PROJECT
                        if sectionId == 0 and ('ProjectName' not in data or 'ProjectNumber' not in data or 'ProjectFile' not in data):
                                data[key] = value
                        
                        # GROUP
                        elif sectionId == 1:
                                tmpUnit[key] = value
                        
                        # MAINPART
                        elif sectionId == 2:
                                if 'MainPart' not in tmpComponent:
                                        tmpComponent['MainComponent'] = True
                                tmpComponent[key] = value
                        
                        # SUBPART
                        elif sectionId == 3:
                                if 'MainPart' not in tmpComponent:
                                        tmpComponent['MainComponent'] = False
                                tmpComponent[key] = value
                        
                        # HOLE 
                        elif sectionId == 4:
                                tmpHole[key] = value
                        
                        # SUBBODY
                        elif sectionId == 5:
                                tmpSubbody[key] = value
                        
                        # TODO: exception? 
                        else:
                                pass 

        # add unclosed parts at the end of the file
        if tmpHole:
                tmpComponent['holes'].append(tmpHole)
        if tmpSubbody:
                tmpComponent['subbodies'].append(tmpSubbody)
        tmpUnit['components'].append(tmpComponent)
        data['units'].append(tmpUnit)

        return data


if __name__ == "__main__":
    f = open('./Parser/data/A106.pl3', 'r')
    input = f.read()
    data = parse(input)
    
    with open('./Parser/json/A106.json', 'w') as outFile:
        json.dump(data, outFile)