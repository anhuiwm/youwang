<html>
<head><title>test sandbox</title>
<style>

</style>
</head>
<body> 

<?php
function guid(){
    if (function_exists('com_create_guid')){
        return com_create_guid();
    }else{
        mt_srand((double)microtime()*10000);//optional for php 4.2.0 and up.
        $charid = strtoupper(md5(uniqid(rand(), true)));
        $hyphen = chr(45);// "-"
        $uuid = chr(123)// "{"
                .substr($charid, 0, 8).$hyphen
                .substr($charid, 8, 4).$hyphen
                .substr($charid,12, 4).$hyphen
                .substr($charid,16, 4).$hyphen
                .substr($charid,20,12)
                .chr(125);// "}"
        return $uuid;
    }
}

$applicationCode = "EwBpnrOhbFaghdYfiKGXgsQyof9A620Q";
 $key = "WbTMKOlE0XADE2vCFhYuGPndLC9S6zxn";
  $data['applicationCode'] = $applicationCode;
   $data['referenceId'] = guid();
     $data['version'] = "v1";
// $data['virtualCurrencyName'] = "testing";
   // $data['virtualCurrencyAmount'] = "10";
  $data['returnUrl'] = "http://kygoo.byethost18.com/index.html?referenceId=".$data['referenceId'];
  $data['amount'] = "100";
  //$data['channelId'] = "";
  $data['currencyCode'] = "MYR";
  $data['description'] = "SandboxTest";
  $data['customerId'] = "customer01";
 $signature = md5($data['amount'].$data['applicationCode'].$data['currencyCode'].$data['customerId'].$data['description'].$data['referenceId'].$data['returnUrl'].$data['version'].$key);

  $data['signature'] =  $signature;
 $databody = http_build_query($data);
 $url="https://sandbox.api.mol.com/payout/payments";
 
$ch = curl_init();
 curl_setopt($ch, CURLOPT_POSTFIELDS, $databody);
  curl_setopt($ch, CURLOPT_URL, $url);
  curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 2);
  curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
  curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
  curl_setopt($ch, CURLOPT_POST, 1);


  
 $result = curl_exec($ch);
 $httpcode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
 echo "<br />".$result;
 echo "<p>".$httpcode."</p>";
 curl_close($ch);
 $json = json_decode($result);
  
 echo "<a href=\"".$json->{'paymentUrl'}."\">Click here</a>";
 
?>
</body>
</html>