// This file has been autogenerated.

exports.setEnvironment = function() {
  process.env['USER_ID'] = 'UK8CH2281:TKGSUQHQE';
  process.env['BOT_ID'] = 'BKGSYSTFG:TKGSUQHQE';
  process.env['HOST_URL'] = 'https://slack.botframework.com';
  process.env['AZURE_SUBSCRIPTION_ID'] = '0389857f-2464-451b-ac83-5f54d565b1a7';
};

exports.scopes = [[function (nock) { 
var result = 
nock('https://slack.botframework.com:443', {"encodedQueryParams":true})
  .filteringRequestBody(function (path) { return '*';})
.post('/v3/conversations', '*')
  .reply(200, {"id":"BKGSYSTFG:TKGSUQHQE:DKE8NUG92"}, [ 'Cache-Control',
  'no-cache',
  'Pragma',
  'no-cache',
  'Content-Length',
  '45',
  'Content-Type',
  'application/json; charset=utf-8',
  'Expires',
  '-1',
  'Server',
  'Microsoft-IIS/10.0',
  'x-ms-request-id',
  '|48e9f8ff0600f64ba9b957cf438c2611.81c15019_',
  'Strict-Transport-Security',
  'max-age=31536000',
  'Date',
  'Wed, 19 Jun 2019 22:23:45 GMT',
  'Connection',
  'close' ]);
 return result; },
function (nock) { 
var result = 
nock('https://slack.botframework.com:443', {"encodedQueryParams":true})
  .filteringRequestBody(function (path) { return '*';})
.post('/v3/conversations/BKGSYSTFG%3ATKGSUQHQE%3ADKE8NUG92/attachments', '*')
  .reply(200, {"id":"at26759-JgIliwm8fIS-e"}, [ 'Cache-Control',
  'no-cache',
  'Pragma',
  'no-cache',
  'Content-Length',
  '37',
  'Content-Type',
  'application/json; charset=utf-8',
  'Expires',
  '-1',
  'Server',
  'Microsoft-IIS/10.0',
  'x-ms-request-id',
  '|4c7b6da60e0ac44e938e5992842faf84.81c1501a_',
  'Strict-Transport-Security',
  'max-age=31536000',
  'Date',
  'Wed, 19 Jun 2019 22:23:45 GMT',
  'Connection',
  'close' ]);
 return result; },
function (nock) { 
var result = 
nock('https://slack.botframework.com:443', {"encodedQueryParams":true})
  .get('/v3/attachments/at26759-JgIliwm8fIS-e')
  .reply(200, {"name":"bot-framework.png","type":"image/png","views":[{"viewId":"original","size":8369}]}, [ 'Cache-Control',
  'no-cache',
  'Pragma',
  'no-cache',
  'Content-Length',
  '142',
  'Content-Type',
  'application/json; charset=utf-8',
  'Expires',
  '-1',
  'Server',
  'Microsoft-IIS/10.0',
  'x-ms-request-id',
  '|89fa5cc5b5afa74c83292f9c7b7e9b1a.81c1501b_',
  'Strict-Transport-Security',
  'max-age=31536000',
  'Date',
  'Wed, 19 Jun 2019 22:23:46 GMT',
  'Connection',
  'close' ]);
 return result; }]];