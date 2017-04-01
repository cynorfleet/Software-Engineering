#!/usr/bin/env python

import threading
import logging
import time
import os.path
from textwrap import TextWrapper

wrapper = TextWrapper(subsequent_indent=' '*10)
localtime = time.asctime( time.localtime(time.time()) )
seperator = ' ------------------------------------- '
header = seperator + localtime + seperator

class FormatterWithHeader(logging.Formatter):
    def __init__(self, header, fmt=None, datefmt=None, style='%'):
        super().__init__(fmt, datefmt, style)
        self.header = header # This is hard coded but you could make dynamic


        # Override the normal format method
        self.format = self.first_line_format

    def first_line_format(self, record):
        # First time in, switch back to the normal format function
        self.format = super().format
        return self.header + "\n" + self.format(record)

class AsyncLog(threading.Thread):
    def __init__(self, output_dir, writemode):

        self.logger = logging.getLogger()
        self.logger.setLevel(logging.DEBUG)

        # create console handler and set level to info
        self.handler = logging.StreamHandler()
        self.handler.setLevel(logging.INFO)
        self.formatter = FormatterWithHeader('')
        self.handler.setFormatter(self.formatter)
        self.logger.addHandler(self.handler)

        # create error file handler and set level to error
        self.handler = logging.FileHandler(os.path.join(output_dir, "error.log"),writemode,
         encoding=None, delay="true")
        self.handler.setLevel(logging.ERROR)
        self.formatter = FormatterWithHeader(self.header)
        self.handler.setFormatter(self.formatter)
        self.logger.addHandler(self.handler)

        # create debug file handler and set level to debug
        if os.path.exists(output_dir) == False:
            os.mkdir(output_dir)
        self.handler = self.logging.FileHandler(os.path.join(output_dir, "all.log"),writemode,
         encoding=None, delay="true")

        self.handler.setLevel(self.logging.DEBUG)
        self.formatter = FormatterWithHeader(self.header)
        self.handler.setFormatter(self.formatter)
        self.logger.addHandler(self.handler)

    # https://goo.gl/txQEbI
    def autolog(self, message):
        "Automatically log the current function details."
        import inspect, logging
        # Get the previous frame in the stack, otherwise it would
        # be this function!!!
        self.func = inspect.currentframe().f_back.f_code
        # Dump the message + the name of this function to the log.
        self.logging.debug("%s in %s:%i\n%s\n" % (
            self.func.co_name,
            self.func.co_filename,
            self.func.co_firstlineno,
            message
        ))
        # https://goo.gl/txQEbI
    def logit(self, lineno, message):
        "Write a msg to a log file."
        import inspect, logging
        self.logging.debug("<line>{lineno}</line> <var>{message}</var>".format(lineno=lineno,var=message))
        return
