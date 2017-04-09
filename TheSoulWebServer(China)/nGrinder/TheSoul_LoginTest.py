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

control = HTTPPluginControl.getConnectionDefaults()
# if you don't want that HTTPRequest follows the redirection, please modify the following option 0.
# control.followRedirects = 1
# if you want to increase the timeout, please modify the following option.
control.timeout = 6000

tests = {}
tests['1'] = Test(1, "login-logout")
#tests['2'] = Test(2, "building_get")
#tests['3'] = Test(3, "officer_list")

# Make any method call on req increase TPS
urlbase = 'http://210.122.11.197:11000/'

class TestRunner:
    account = None
    general = None
    xml_constants = None
    #username = 'user_' + str(random.randint(10000, 99999))
    req = None # req = HTTPRequest()

    def GET(self, url, params = {}):
        global urlbase
        pairs = []
        for k, v in params.iteritems():
            pairs.append(NVPair(k, str(v)))

        pairs.append(NVPair('Debug', str('1')))
        # set cookie before REQUEST if we have
        #if self.cookies:
        #    for cookie in self.cookies:
        #        CookieModule.addCookie(cookie, HTTPPluginControl.getThreadHTTPClientContext())

        result = self.req.GET(urlbase + url, pairs)

        # set cookie after REQUEST if we had not
        #if not self.cookies and result.getStatusCode() == 200:
        #    self.cookies = CookieModule.listAllCookies(HTTPPluginControl.getThreadHTTPClientContext())

        return result

    def expect(self, substring, response, code=None):   
        assert response.getStatusCode() == 200, 'response.getStatusCode(): ' + repr(response.getStatusCode())
        try:
            js = json.loads(response.getText())
        except Exception, e:
            grinder.logger.error('response.text: ' + str(response.text))
            raise e

        #if code and len(code) > 0:
        #    assert code in js['code'], pretty(js)
        #elif substring and len(substring) > 0:
        if substring and len(substring) > 0:
            grinder.logger.debug('EXPECTS SUBSTR: ' + substring + '...')
        #    assert substring in js['message'], pretty(js)
        return js

    def user_login(self):
        global tests
        self.req = HTTPRequest()
        tests['1'].record(self.req)

        e = {'op': 'login'}
        e['aid'] = self.aid;
        #e = {'username': self.username, 'password': self.password}
        #e['country'] = 1 if self.allies else 2
        #e['market_type'] = self.market_type
        #e['ssl'] = True
        #e['email'] = self.username + '@test.ee.com'

        r = self.GET('RequestAccount.aspx', e)        
        js = self.expect(None, r)

        reterror = js['resultcode'];

        if reterror == 0:
            self.account = js['account']
        # if 'already exists' not in js['message']:
        #    r = self.GET('auth/logout.php', e)
        #    js = self.expect(None, r)
        #   e['xml_constants'] = 1

        #r = self.GET('auth/login.php', e)
        #js = self.expect(None, r)

        #if 'user' not in js:
        #   grinder.logger.error('js: ' + str(js))

        #self.user = js['user']
        #self.constants = js['constants']
        #if 'xml_constants' in js:
        #   self.xml_constants = js['xml_constants']

        #r = self.GET('general/general.php', e)
        #js = self.expect(None, r)
        #self.general = js['general']
        grinder.logger.info('logged in : %d' % (self.aid))
        return js

    def user_logout(self):
        #global tests
        #self.req = HTTPRequest()
        #tests['1'].record(self.req)
        #e = {'op': 'logout','aid': self.aid}

        #if self.account:
        #    r = self.GET('RequestAccount.aspx', e)        
        grinder.logger.info('logged out : %d' % (self.aid))

    # initlialize a thread 
    def __init__(self):
        #grinder.logger.debug('%d %d / %d %d %d' % (self.totalProcessCount, self.totalThreadCount, grinder.agentNumber, grinder.processNumber, grinder.threadNumber))
        #totalProcessCount = grinder.getProperties().getInt("grinder.processes", 1);
        #totalThreadCount = grinder.getProperties().getInt("grinder.threads", 1);
        #self.aid = totalThreadCount;
        #self.username = 'u_%08d' % ((grinder.agentNumber * totalProcessCount * totalThreadCount) + (grinder.processNumber * totalThreadCount) + grinder.threadNumber)
        grinder.statistics.delayReports=True
        #self.user_login()

    def __del__(self):
        self.user_logout()

    # test method        
    def __call__(self):
       #totalThreadCount = grinder.getProperties().getInt("grinder.threads", 1);
       self.aid = grinder.threadNumber + 1;
       js = self.user_login();
       if js:
           grinder.statistics.forLastTest.success = 1;
        #rnd = random.random()
        #if rnd < 0.3: js = self.building_get()
        #else: js = self.officer_list()

        #if js:
        #    grinder.statistics.forLastTest.success = 1
        #else:
        #    grinder.statistics.forLastTest.success = 0