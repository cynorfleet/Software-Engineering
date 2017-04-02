#!/usr/bin/env python
'''     TESTCASE.PY
        To use this program call it by `testcase.py -s <sourcepath> -o <outfilename>`
        It will read a source file and parse it against the defined GLOBALS
        It will then write to a `.dat` file for the native C# program to use as data    '''

import re, sys, optparse
#   GLOBALS:        these variables will be used when FINDING KEYWORD in user code
#   strprse =       finds alphanumeric keyword (this includes array indicies)
#   inputdic =      specfies which keyword to look for
blacklist   = ['int main']
closure     = '[ ,;]'
functypes   = ['void', 'int', 'string', 'char']
strprse     = '([a-zA-Z0-9 ]*([\[a-zA-Z0-9\]]*))'
inputtype   = {'int':re.compile(' *(int[ ]+{key}){end}'.format(key=strprse, end=closure))}
inputdic    = {'cin':re.compile(' *cin[ >]+{key}'.format(key=strprse)),
                'getline':re.compile('.*getline[ \(]+{key}[ ,]+[a-zA-Z0-9 ]+\)'.format(key=strprse))}

def getFile(file):
    # GETFILE:        this function will GET ALL LINES in a FILE
    try:
        with open (file, "r") as myfile:    # open the file temporarily
            linelist=myfile.readlines()     # store lines as an array
    except:                                 # if something crazy happend
        print("File Not Found")
        exit(2)                             # leave the program
    return linelist

def funcparse(lines):
    # FUNCPARSE:       this function will GET FUNCTION declarations in each LINE
    global functypes, strprse                   # allows FUNCPARSE to use GLOBALS
    resultlist = []                             # arry list to hold results
    for i, line in enumerate(lines):            # go thru each line and hold i(line#) and line(line data)
        for functype in functypes:              # for each function type in GLOBALS
            parser = re.compile(' *({type} *{keyword} *\(.*\))'.format(type=functype, keyword=strprse))
            match = re.match(parser, line)      # look for the func type
            if match != None:                   # if match was found
                match = match.group(1)          # throw away garbage
                resultlist.append((i+1, match)) # add line number and match to results
    return resultlist

def parseLines(lines, keyword, strip=None):
    # PARSELINE:      this function will FIND USER VARIABLES in each LINE
    resultlist = []
    for i, line in enumerate(lines):             # for every line in the file
        result = re.match(keyword, line)         # search for var and put in group 1
        if result != None:                       # if a match was found
            result = result.group(1)             # throw away everythong but group 1
            if strip != None:                    # if we need to remove whitespace
                results = result.replace(' ','') # remove whitespaces
            if result not in blacklist:          # if not a default header (ie. 'int Main()')
                resultlist.append((i+1,result))  # add result to a list
    return resultlist

def showMatches(matchtype, matches, log):
    # SHOWMATCHES:    this function will PRINT the MATCHES
    log.write("--------------------------------------------\n")
    log.write("{type}:\n".format(type=matchtype))           # print header
    for i, match in enumerate(matches):                     # for each match in list
        log.write("{match}\n".format(match=matches[i]))     # print the match
    return


def main(args):
    # MAIN:           AND HERE... WE... GO
    (options, args) = ParseOpt()            # parses the CLI options to get source out paths
    options.outfile = '{file}.dat'.format(file=options.outfile)   # adds ".dat" to filename
    outbuffer = []                          # holds the data to be writted to outfile

    lines = getFile(options.source)         # stores the lines in file as arry list
    functions = funcparse(lines)            # look thru each line and finds function declarations
    datatype = parseLines(lines, inputtype['int']) # look thru each line and find data types
    for intype, parser in inputdic.items(): # for every input command in the code
        inputs = parseLines(lines, parser)  # look thru each line and find user inputs
        outbuffer.extend(inputs)            # add the results to a outfile buffer to write later

    with open(options.outfile, 'w') as outfile:     # open the outfile for writing
        showMatches('Functions', functions, outfile)    # write the functions
        showMatches('Type', datatype, outfile)      # write the DataTypes
        showMatches('Inputs', outbuffer, outfile)   # write the inputs
    return 0

def ParseOpt():
    # PARSEOPT:     this function will process the Cli args (no big deal)
    parser = optparse.OptionParser('\n%prog -s <source> -o <outfile>',
    version="%prog 1.0")
    parser.add_option('-s', dest='source', type='string', default='main.cpp',
    help='<source> is the name out the file you wish to parse')
    parser.add_option('-o', dest='outfile', type='string', default='outfile.dat',
    help='<outfile> is the name out the file you wish store results')
    return parser.parse_args()

if __name__ == '__main__':                  # if program was not called by another module (parent prog)
    sys.exit(main(sys.argv))                # call main and send CLI args

__author__ = "Christian Norfleet"
__copyright__ = "Copyright 2017, Christian Norfleet"
__credits__ = ["Cavaughn Browne", "Damien Moeller", "Aimee Phillips",]
__license__ = "GPL"
__version__ = "1.0.1"
__maintainer__ = "Christian Norfleet"
__email__ = "cynorfleet0120@my.mwsu.edu"
__github__ = "https://github.com/cynorfleet/Software-Engineering.git"
__status__ = "Production"
