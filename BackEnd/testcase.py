#!/usr/bin/env python
import re

#   GLOBALS:        these variables will be used when FINDING KEYWORD in user code
#   strprse =       finds alphanumeric keyword (this includes array indicies)
#   inputdic =      specfies which keyword to look for
closure     = '[ ,;]'
strprse     = '([a-zA-Z0-9 ]*([\[a-zA-Z0-9\]]*))'
inputtype   = {'int':re.compile(' *(int[ ]+{}){}'.format(strprse, closure))}
inputdic    = {'cin':re.compile(' *cin[ >]+{}'.format(strprse)),
                'getline':re.compile('.*getline[ \(]+{}[ ,]+[a-zA-Z0-9 ]+\)'.format(strprse))}
functypes   = ['void', 'int', 'string', 'char']
blacklist   = ['int main']

# GETFILE:        this function will GET ALL LINES in a FILE
def getFile(file):
    with open (file, "r") as myfile:    # open the file temporarily
        linelist=myfile.readlines()     # store lines as an array
    return linelist                     #return the array

def funcparse(lines):
    global functypes, strprse
    resultlist = []
    for i, line in enumerate(lines):
        for functype in functypes:
            parser = re.compile(' *({type} *{keyword} *\(.*\))'.format(type=functype, keyword=strprse))
            match = re.match(parser, line)
            if match != None:
                match = match.group(1)
                resultlist.append((i+1, match))
    return resultlist

# PARSELINE:      this function will FIND USER VARIABLES in each LINE
def parseLines(lines, keyword, strip=None):
    resultlist = []
    for i, line in enumerate(lines):                            # for every line in the file
        result = re.match(keyword, line)         # search for var and put in group 1
        if result != None:                     # if a match was found
            result = result.group(1)
            if strip != None:                   # throw away everythong but group 1
                results = result.replace(' ','')   # remove whitespaces
            if result not in blacklist:
                resultlist.append((i+1,result))         # add result to a list
    return resultlist


# SHOWMATCHES:    this function will PRINT the MATCHES
def showMatches(matchtype, matches, log):
    log.write("--------------------------------------------\n")
    log.write("{type} Found:\n".format(type=matchtype))   # print header
    for i, match in enumerate(matches):             # for each match in list
        log.write("{match}\n".format(match=matches[i]))   # print the match
    return


# MAIN:           AND HERE... WE... GO
def main():
    inputbuffer = []
    log = open('inputs.log', 'w')
    filepath = input("Enter File Path: ")
    lines = getFile(filepath)

    functions = funcparse(lines)
    datatype = parseLines(lines, inputtype['int'])
    for intype, parser in inputdic.items():
        inputs = parseLines(lines, parser)
        print('INPUTS {} for dict {}\n'.format(inputs, intype))
        inputbuffer.extend(inputs)

    showMatches('Functions', functions, log)
    showMatches('Type', datatype, log)
    showMatches('Inputs', inputbuffer, log)

    print("\nDONE")
    return 0

if __name__ == '__main__':
    main()
