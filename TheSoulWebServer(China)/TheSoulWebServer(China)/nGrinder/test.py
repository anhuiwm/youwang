# -*- coding:utf-8 -*-

# A simple example using the HTTP plugin that shows the retrieval of a
# single page via HTTP. 
#
# This script is automatically generated by ngrinder.
#
# @author admin
from net.grinder.script.Grinder import grinder
from net.grinder.script import Test
from net.grinder.plugin.http import HTTPRequest
from net.grinder.plugin.http import HTTPPluginControl
from HTTPClient import NVPair

from HTTPClient import Cookie, CookieModule, CookiePolicyHandler
from org.json import JSONObject
import random, datetime, os, copy, logging, time
import com.xhaus.jyson.JysonCodec as json
from java.lang import System

control = HTTPPluginControl.getConnectionDefaults()
# if you don't want that HTTPRequest follows the redirection, please modify the following option 0.
# control.followRedirects = 1
# if you want to increase the timeout, please modify the following option.
control.timeout = 6000

test1 = Test(1, "Test1")
request1 = HTTPRequest()

# Make any method call on request1 increase TPS
test1.record(request1)

class TestRunner:
    # initlialize a thread 
    def __init__(self):
        grinder.statistics.delayReports=True
        pass

    # test method        
    def __call__(self):
        result = request1.GET("http://google.com")
        
        # You get the message body using the getText() method.
        # if result.getText().find("HELLO WORLD") != -1 :
        #    grinder.statistics.forLastTest.success = 1
        # else :
        #     grinder.statistics.forLastTest.success = 0
            
        # if you want to print out log.. Don't use print keyword. Instead, use following.
        # grinder.logger.info("Hello World")
        getparams = System.getProperty("param", "1");
        unilist = getparams.encode('utf-8').split(",")        
        testlist = map(int, unilist)
        grinder.logger.info("getparams : " + getparams)
        grinder.logger.info("unilist : " + str(unilist))
        grinder.logger.info("testlist : " + str(testlist))

        grinder.logger.info("json dump" + json.dump(testlist))
        if result.getStatusCode() == 200 :
            grinder.statistics.forLastTest.success = 1
        elif result.getStatusCode() in (301, 302) :
            grinder.logger.warn("Warning. The response may not be correct. The response code was %d." %  result.getStatusCode()) 
            grinder.statistics.forLastTest.success = 1
        else :
            grinder.statistics.forLastTest.success = 0
