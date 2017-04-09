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

   $applicationCode = "FNWqwwgGrrtR5Bg5l6dcnEigVb3yenTC";//"09CPNLtJ1IDNwpXTu8hC7Z3OnUPK8b7R";
   $key = "XhtNbbDYOOQo5TG5I621KWFDs03dBKqU";//"x9UmhiQgyfVhbMuqR8EU7w3lKrmh807o";
$data['applicationCode'] = $applicationCode;
$data['referenceId'] = guid();
$data['SerialNo'] = "7516010618";
$data['Pin'] = "99997516010618";
$data['description'] = "SandboxTest";
$data['customerId'] = "customer01";
$data['currencyCode'] = "MYR";
$data['ClientIpAddress'] = "http://107.150.101.9:5100/iosMolBilling.aspx";
$data['version'] = "v1";
 
 
  ksort($data);
  
   $str = "";
  foreach ($data as $da){ 
      $str = $str.$da;
    } 
	echo $str."\n";
 $signature = md5($str.$key);
 //$signature = md5($data['amount'].$data['applicationCode'].$data['currencyCode'].$data['customerId'].$data['description'].$data['referenceId'].$data['returnUrl'].$data['version'].$key);

  $data['signature'] =  $signature;
 $databody = http_build_query($data);
 $url="https://api.mol.com/payout/payments/molpoints/pin";
 
// Sandbox
// https://sandbox.api.mol.com/payout/payments/molpoints/pin
// Production
// https://api.mol.com/payout/payments/molpoints/pin

$ch = curl_init();
 curl_setopt($ch, CURLOPT_POSTFIELDS, $databody);
  curl_setopt($ch, CURLOPT_URL, $url);
  curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 2);
  curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
  curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
  curl_setopt($ch, CURLOPT_POST, 1);


  
 $result = curl_exec($ch);
 $err = curl_error($ch);
  echo 'Curl error: ' . curl_error($ch)."\n";
file_put_contents("log_charge.txt", $result.'###'.$err.PHP_EOL, FILE_APPEND);

 $httpcode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
 echo "::".$result."\n";
 echo "::".$httpcode."\n";
 curl_close($ch);
 //$json = json_decode($result);
  
 //echo "<a href=\"".$json->{'paymentUrl'}."\">Click here</a>";
 
?>
</body>
</html>