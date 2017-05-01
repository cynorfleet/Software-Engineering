#!/usr/bin/env python
'''     TESTCASE.PY
        To use this program call it by `testcase.py -s <sourcepath> -o <outfilename>`
        It will read a source file and parse it against the defined GLOBALS
        It will then write to a `.dat` file for the native C# program to use as data    '''

import re, sys
#   GLOBALS:        these variables will be used when FINDING KEYWORD in user code
#   strprse =       finds alphanumeric keyword (this includes array indicies)
#   inputdic =      specfies which keyword to look for
results = []
blacklist   = ['int main']
closure     = '[ ,;]'
functypes   = ['void', 'int', 'string', 'char', 'double', 'float']
strprse     = '([a-zA-Z0-9 ]*([\[a-zA-Z0-9\]]*))'
strprse2    = '([a-zA-Z][a-zA-Z0-9_]*)'
strprse3    = '(>>)[ \t]+([a-zA-Z][a-zA-Z0-9_]*)'
relation    = ['<', '>', '<=', '>=', '==', '!=']
logical     = ['&&', '||']
inputdic    = {'stdIn':re.compile('{key}'.format(key=strprse3)),
                'getline':re.compile('getline[ \t\(]+{key}[ \t,]+{key}+\)'.format(key=strprse2))}
               
def getFile(file):
    # GETFILE:        this function will GET ALL LINES in a FILE
    try:
        with open (file, "r") as myfile:    # open the file temporarily
            linelist=myfile.readlines()     # store lines as an array
    except:                                 # if something crazy happend
        print("File Not Found")
        exit(2)                             # leave the program
    return linelist

def funcparse(lines, vartype=None, getcin=False):
    # FUNCPARSE:       this function will GET FUNCTION declarations in each LINE
    global functypes, strprse                   # allows FUNCPARSE to use GLOBALS
    resultlist = []                             # arry list to hold results
    className = ""
    match = None
    for i, line in enumerate(lines):            # go thru each line and hold i(line#) and line(line data)
        for functype in functypes:              # for each function type in GLOBALS
            if getcin == False:
                parser = re.compile('[ \t]*({type} *{keyword} *\(.*\))'.format(type=functype, keyword=strprse))
                match = re.match(parser, line)      # look for the func type
            else:
                parser =  vartype
                match = re.match(parser, line)      # look for the func type
            if match != None and match.group(1) not in resultlist:                   # if match was found
                match = match.group(1)          # throw away garbage
                resultlist.append(match) # add line number and match to results
                
        parser = re.compile('class[ \t]+{key}'.format(key=strprse2)); 
        match = re.match(parser, line)            #look for class name
        
        if(match != None):
            className = match.group(1)
            className = className.replace(' ', '')
            print ("ClassName: {c}".format(c=className))
    return resultlist, className

def parseCinGet(lines, keyword, strip=None):
    # PARSELINE:      this function will FIND USER VARIABLES in each LINE
    resultlist = []
    for i, line in enumerate(lines):            # for every line in the file
        parser = keyword
        #result = re.match(parser, line)         # search for var and put in group 1
        result = re.findall(parser, line)       #returns a list of tuples containing groups
        for r in result:                        #for each tuple in the list
            if r != None:                       # if a match was found
                r = r[1]                        # throw away everythong but group 1
                if strip != None:               # if we need to remove whitespace
                    r = r.replace(' ','')       # remove whitespaces
                if r not in blacklist:          # if not a default header (ie. 'int Main()')
                    print("cin: {}".format(r))  
                    resultlist.append(r)        # add result to a list
    return resultlist
                                               
def parseIfInputs(lines, inputs):              #parseIfInputs looks for inputs that are arguments of if statements
    resultList = []                            #PROTOTYPE: DOESN'T WORK AS EXPECT. (IDEA SCRAPPED)
    wordList = []  
    for i, line in enumerate(lines):
        words = line.split()
        print ("Words: ", words)     
        if len(words) != 0:       
            if words[0] == "if" or words[0] == "else if":                     
                for word in words[1:]:                  #compare each word to the inputs              
                    if word in logical:
                         resultList.append(wordList)    # add results to a list
                         wordList = []                        
                    word = word.replace('(', '')
                    word = word.replace(')', '')                   
                    if word in relation:
                        wordList.append(word)                      
                    for i in inputs:                   
                        if word == i:
                            wordList.append(word)                           
                resultList.append(wordList)              # add result to a list
                print ("Result: ", wordList)                      
    return resultList
                        
    

def parseLines(lines, strip=None):
    # PARSELINE:      this function will FIND USER VARIABLES in each LINE
    resultlist = []
    for i, line in enumerate(lines):                # for every line in the file
        for functype in functypes[1:]:              # for each function type in GLOBALS
            parser = re.compile('[ \t]*({type}(?:[ \t]*{key}[ \t]*(?:,|;))+)'.format(type=functype, key=strprse2))
            result = re.findall(parser, line)       #returns a list of tuples containing groups          
            for r in result:                        #for each tuple in the list
                if r != None:                       # if a match was found
                    r = r[0]                        # throw away everything but group 0
                    words = r.split()
                    for word in words:              #get each declaration variable in r
                        if word != functype:
                            word = word.replace(',', '') #remove any commas
                            word = word.replace(';', '') #remaove any semicolons
                            word = functype + " " + word
                        if word not in blacklist and word not in resultlist:  # if not a default header (ie. 'int Main()')
                            resultlist.append(word)  # add result to a list

    return resultlist

def showMatches(matchtype, matches, log):
    # SHOWMATCHES:    this function will PRINT the MATCHES
    log.write("--------------------------------------------\n")
    log.write("{type}:\n".format(type=matchtype))           # print header
    for i, match in enumerate(matches):                     # for each match in list
        log.write("{match}\n".format(match=matches[i]))     # print the match
    return

def sendDataBack(structure, inputs):
    dataTypeList = []
    varList = []
    for variable in inputs:
        for i, dtype in enumerate(structure):
            if re.match('.* {var}.*'.format(var=variable), dtype) != None and variable not in varList:
                dataTypeList.append(dtype.split(" ")[0])
                varList.append(variable)
    return (dataTypeList,varList)

# MAIN:           AND HERE... WE... GO
outfile = '{file}.dat'.format(file=sys.argv[1])   # adds ".dat" to filename
inputs = []                          # holds the data to be writted to outfile

lines = getFile(sys.argv[0])         # stores the lines in file as arry list
functions, className = funcparse(lines)            # look thru each line and finds function declarations
print(className)

datatype = []
datatype.extend(parseLines(lines)) # look thru each line and find data types


for intype, parser in inputdic.items(): # for every input command in the code
    # var = parseLines(lines, parser)  # look thru each line and find user inputs
    # inputs.extend(var)            # add the results to a outfile buffer to write later
    var = parseCinGet(lines, parser, True)  # look thru each line and find user inputs
    inputs.extend(var)            # add the results to a outfile buffer to write later
    

datatype, userinputs = sendDataBack(datatype, inputs)

with open(outfile, 'w') as outfile:     # open the outfile for writing
    showMatches('Functions', functions, outfile)    # write the functions
    showMatches('Type', datatype, outfile)      # write the DataTypes
    showMatches('Inputs', inputs, outfile)   # write the inputs

for i in datatype:
    print(i)
for i in inputs:
    print(i)




__author__ = "Christian Norfleet"
__copyright__ = "Copyright 2017, Christian Norfleet"
__credits__ = ["Cavaughn Browne", "Damien Moeller", "Aimee Phillips",]
__license__ = "GPL"
__version__ = "1.0.1"
__maintainer__ = "Christian Norfleet"
__email__ = "cynorfleet0120@my.mwsu.edu"
__github__ = "https://github.com/cynorfleet/Software-Engineering.git"
__status__ = "Production"
