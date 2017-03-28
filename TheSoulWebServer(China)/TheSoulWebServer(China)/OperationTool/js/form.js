var isRunnig;
isRunnig = false;

function formsendCheck() {
    if (isRunnig) {
        alert("正在处理中, 请稍等.");
        return false;
    }
    else {
        isRunnig = true;
        return true;
    }
}

function defaultSubmit(name, mode) {
    if (isRunnig) {
        alert("正在处理中, 请稍等.");
        return false;
    }
    else {
        isRunnig = true;
        $("input:hidden[id=" + name + "]").val(mode);
        document.forms[0].submit();
    }
}
function rankingDelete(idx) {
    if (isRunnig) {
        alert("正在处理中, 请稍等.");
        return false;
    }
    else {
        isRunnig = true;
        $("input:hidden[id=userid]").val(idx);
        formsendCheck();
        document.forms[0].submit();
    }
}

function blilingStatus(idx) {
    if (isRunnig) {
        alert("正在处理中, 请稍等.");
        return false;
    }
    else {
        isRunnig = true;
        $("input:hidden[id=billingindex]").val(idx);
        formsendCheck();
        document.forms[0].submit();
    }
}


function tableAdd(table) {
    // clone
    var index = 0;
    $("#" + table + " > tbody").children('tr').each(function () {
        index = index + 1
    });
    //if (index < 10) {
        $.trClone = $("#" + table + " tr:last").clone().html();
        $.newTr = $("<tr>" + $.trClone + "</tr>");
        // append
        $("#" + table + "").append($.newTr);
    //}
    //else {
    //    alert("최대 10개까지 지급가능합니다.");
    //}
}

function golink(url, urlparam) {
    location.href = url + urlparam;
}

function golinkPost(url, urlparam) {
    document.forms[0].action = url + urlparam;
    document.forms[0].method = "post";
    document.forms[0].submit();
}

function popup_itemadd(itemid, itemName, grade, level, cnt) {
    opener.itemadd(itemid, itemName, grade, level, cnt);
    self.close();
}

function deleteTr(obj) {
    var tr = $(obj).parent().parent();
    tr.remove();

    var index = 0;
    $("#itemtable > tbody").children('tr').each(function () {
        index = index + 1
    });
    if (index < 6) {
        $("#itemAddButton").attr('disabled', false);
    }
    else {
        $("#itemAddButton").attr('disabled', true);
    }
}

function openItemPop() {
    var index = 0;
    $("#itemtable > tbody").children('tr').each(function () {
        index = index + 1
    });
    if (index < 6) {
        $("#itemAddButton").attr('disabled', false);
        window.open('/User/pop_itemCharge.aspx', '_blank', 'width=500, height=400, toolbar=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no');
    }
    else {
        $("#itemAddButton").attr('disabled', true);
    }
}

function itemCharge() {
    alert($("input:hidden[name=itemid]").index(0).val());
}

function itemadd(itemid, itemName, grade, level, cnt) {
    if (itemName != "") {
        if ($.trim($("#itemid").val()) == "") {
            $("#itemid").val(itemid);
            $("#itema").val(cnt);
            $("#level").val(level);
            $("#grade").val(grade);
        }
        else {
            var str_itemid = $("#itemid").val() + "," + itemid;
            var str_itema = $("#itema").val() + "," + cnt;
            var str_grade = $("#grade").val() + "," + grade;
            var str_level = $("#level").val() + "," + level;
            $("#itemid").val(str_itemid);
            $("#itema").val(str_itema);
            $("#level").val(str_level);
            $("#grade").val(str_grade);
        }
        $.newTr = $("<tr><td><input type=\"hidden\" name=\"itemid\" value='" + itemid + "' /><input type=\"hidden\" name=\"itema\" value=\"" + cnt + "\" /><input type=\"hidden\" name=\"level\" value=\"" + level + "\" /><input type=\"hidden\" name=\"grade\" value=\"" + grade + "\" />" + itemName + "</td><td>" + level + "</td><td>" + grade + "</td><td>" + cnt + "</td><td><button type=\"button\" onclick=\"deleteTr(this)\" class=\"btn\">Del</button></td></tr>");

        $("#itemtable").append($.newTr);

        var index = 0;
        $("#itemtable > tbody").children('tr').each(function () {
            index = index + 1
        });
        if (index < 6) {
            $("#itemAddButton").attr('disabled', false);
        }
        else {
            $("#itemAddButton").attr('disabled', true);
        }
    }
}

function payTypeChage(data) {

    if (data == "PriceType_PayReal") {
        $("#pay1").hide();
        $("#pay2").show();
        $("#vipPoint").attr("readonly", false);
    }
    else {
        $("#pay1").show();
        $("#pay2").hide();

        $("#vipPoint").val("0");
        $("#vipPoint").attr("readonly",true);
            
    }
}

function checkRewardCount(item) {
    var $rewardData = $(".rewardItem")

    var itemCount = 0;
    for (var i = 0; i < $rewardData.length; i++) {
        if ($rewardData.eq(i).val() > 0) {
            itemCount = itemCount + 1;
        }
    }
    
    if (itemCount > 5) {
        $("#" + item + " option:eq(0)").attr("selected", "selected");
    }
}

function makeorderID() {
    if ($("input:checkbox[id='chkAutoOrder']").is(":checked") == true) {
        $("#orderID").attr("readonly", true);
        var activeID1 = $("#triggerid1").val();
        var activeID2 = $("#triggerid2").val();
        var clear1_1 = $("#clear1_1").val() == "" ? 0 : parseInt($("#clear1_1").val());
        var clear1_2 = $("#clear1_2").val() == "" ? 0 : parseInt($("#clear1_2").val());
        var clear1_3 = $("#clear1_3").val() == "" ? 0 : parseInt($("#clear1_3").val());
        var clear2_1 = $("#clear2_1").val() == "" ? 0 : parseInt($("#clear2_1").val());
        var clear2_2 = $("#clear2_2").val() == "" ? 0 : parseInt($("#clear2_2").val());
        var clear2_3 = $("#clear2_3").val() == "" ? 0 : parseInt($("#clear2_3").val());
        var clearValue1 = clear1_1 + clear1_2 + clear1_3;
        var clearValue2 = clear2_1 + clear2_2 + clear2_3;
        $("#orderID").val(activeID1 + activeID2 + clearValue1 + clearValue2);
    }
    else {
        $("#orderID").attr("readonly", false);
    }
}

function onlyNumber() {
    $(".onlyNumber").bind("keydown keyup keypress blur", function () {
        $(this).val($(this).val().replace(/[^0-9:\-]/gi, ""));
    });
}

function maxCnt(cnt) {
    alert(cnt);
    $(".maxCnt").bind("keydown keyup keypress blur", function (event) {
        var inputVal = $(this).val();
        if (inputVal > cnt) {
            $(this).val(cnt);
        }
    });
}

function serverChecked() {
    if ($("input:checkbox[name='All_Server']").is(":checked") == true) {
        $('input:checkbox[name="serverid"]').attr("checked", true);
    }
    else {
        $('input:checkbox[name="serverid"]').attr("checked", false);
    }
}