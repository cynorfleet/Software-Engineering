#!/usr/bin/env python
import testformatter, re

''' GLOBALS:        these variables will be used when FINDING KEYWORD in user code
    strprse =       finds alphanumeric keyword (this includes array indicies)
    inputdic =      specfies which keyword to look for '''
strprse  = '*(([a-zA-Z0-9 ])*(\[.*\]))*[ ;]'
inputdic = {'cin':re.compile('cin[ > ]{}'.format(strprse)),
            'cout':re.compile('cout[ < ]{}'.format(strprse))}

''' GETFILE:        this function will GET ALL LINES in a FILE '''
def getFile(file):
    with open (file, "r") as myfile:    # open the file temporarily
        linelist=myfile.readlines()     # store lines as an array
    return linelist                     #return the array

''' PARSELINE:      this function will FIND USER VARIABLES in each LINE '''
def parseLine(list, keyword):
    for i in list:                              # for every line in the file
        results = re.search(keyword, i)         # search for var and put in group 1
        if results != None:                     # if a match was found
            results = results.group(1)          # throw away everythong but group 1
            results = results.replace(' ','')   # remove whitespaces
            return results

''' SHOWMATCHES:    this function will PRINT the MATCHES'''
def showMatches(matchtype, match):
    print("{type} Found: {match}\n".format(type=matchtype, match=match))
    return

''' MAIN:           AND HERE... WE... GO'''
def main():
    filepath=raw_input("Enter File Path: ")
    lines = getFile(filepath)

    inputs = parseLine(lines, inputdic['cin'])
    outputs = parseLine(lines, inputdic['cout'])

    showMatches('Inputs', inputs)
    showMatches('Outputs', outputs)

    print("DONE")
    return 0

if __name__ == '__main__':
    main()
