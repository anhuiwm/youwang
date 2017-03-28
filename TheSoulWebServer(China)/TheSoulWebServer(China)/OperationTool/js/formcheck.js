$('#mcheck').click(function () {
    if ($('#gdiv').val() == "") {
        alert("Please select a game menu.");
        $('#gdiv').focus();
        return false;
    }
    else if ($('#mdiv').val() == "") {
        alert("Please select a main menu.");
        $('#mdiv').focus();
        return false;
    }
    else if ($('#viewidx').val() == "") {
        alert("Please select a view index.");
        $('#viewidx').focus();
        return false;
    }
    else if ($('#menuname').val() == "") {
        alert("input menu name.");
        $('#menuname').focus();
        return false;
    }
    else if ($('#linkurl').val() == "") {
        alert("input link url.");
        $('#linkurl').focus();
        return false;
    }
    else {
        document.frmadd.action = "domenu.asp";
        document.frmadd.submit();
    }
});

function TnSearchUser() {
    if (document.sfrm.userid.value == "") {
        alert("Input userid.");
        document.sfrm.userid.focus();
    }
    else {
        document.sfrm.submit();
    }
}

function Search01() {
    
    var Mindate = $("input:hidden[name=minDate]").val();
    var datecnt;
    if ($("input:text[name=sdate]").length > 0) {
        datecnt = DateDiff(Mindate, $("input:text[name=sdate]").val());
    }
    else {
        datecnt = 0;
    }
    if ($("input:radio[name=SearchType]:input[value=sno]").is(":checked") == true) {
        if ($.isNumeric($("input:text[name=SearchValue]").val()) == false) {
            alert("회원번호는 숫자로 검색가능합니다.");
            return false;
        }
    }
    if ($("input:text[name=SearchValue]").val() == "") {
        alert("Input userid.");
        $("input:text[name=SearchValue]").focus();
    }
    else if (datecnt > 0) {
        alert("한달 내 데이터만 검색 가능합니다.");
    }
    else {
        document.search.action = "";
        document.search.target = "_self";
        document.search.submit();
    }
}
function DateDiff(date1, date2) {
    var arrDate1 = date1.split("-");
    var getDate1 = new Date(parseInt(arrDate1[0]), parseInt(arrDate1[1]) - 1, parseInt(arrDate1[2]));
    var arrDate2 = date2.split("-");
    var getDate2 = new Date(parseInt(arrDate2[0]), parseInt(arrDate2[1]) - 1, parseInt(arrDate2[2]));

    var getDiffTime = getDate1.getTime() - getDate2.getTime();
    var returnValue = Math.floor(getDiffTime / (1000 * 60 * 60 * 24));
    return returnValue;
};
function Search02() {
    if ($("input:radio[name=SearchType]:input[value=all]").is(":checked") == true) {
        document.search.submit();
    }
    else if ($("input:radio[name=SearchType]:input[value=sno]").is(":checked") == true) {
        if ($.isNumeric($("input:text[name=SearchValue]").val()) == false) {
            alert("회원번호는 숫자로 검색가능합니다.");
            return false;
        }
    }
    else {
        if (document.search.SearchValue.value == "") {
            alert("Input userid.");
            document.search.SearchValue.focus();
        }
        else {
            document.search.submit();
        }
    }
}
function paging(page) {
    document.paging.page.value = page;
    document.paging.action = "";
    document.paging.target = "_self";
    document.paging.submit();
}

function getOptionType(option) {
    if (option == "ParameterType_STR") {
        return "힘";
    }
    else if (option == "ParameterType_STA") {
        return "체력";
    }
    else if (option == "ParameterType_MEN") {
        return "정신력";
    }
    else if (option == "ParameterType_LUK") {
        return "행운";
    }
    else if (option == "ParameterType_PRT") {
        return "보호";
    }
    else if (option == "ParameterType_HP" || option == "ParameterType_RATE_HP") {
        return "생명력";
    }
    else if (option == "ParameterType_MP" || option == "ParameterType_RATE_MP") {
        return "마력";
    }
    else if (option == "ParameterType_AP" || option == "ParameterType_RATE_AP") {
        return "공격력";
    }
    else if (option == "ParameterType_DP" || option == "ParameterType_RATE_DP") {
        return "방어력";
    }
    else if (option == "ParameterType_CRT" || option == "ParameterType_RATE_CRT") {
        return "치명타 확률";
    }
    else if (option == "ParameterType_CP" || option == "ParameterType_RATE_CP") {
        return "치명타 보호";
    }
    else if (option == "ParameterType_CR") {
        return "치명타 데미지율";
    }
    else if (option == "ParameterType_PD") {
        return "관통 데미지";
    }
    else {
        return "";
    }
}

function maxCnt(cnt) {
    $(".maxCnt").bind("keydown keyup keypress blur", function (event) {
        var inputVal = $(this).val();
        if (inputVal > cnt) {
            alert("최대 " + cnt + " 까지 입력가능합니다.");

            $(this).val(cnt);
        }
    });
}

function ChanceEventView(idx) {
    var urllink = "../event/chanceEvent_view.asp?idx=" + idx;

    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {

                    $("input:hidden[name=idx]").val(list[0].idx);
                    $("input:text[name=sdate]").val(list[0].sdate.substr(0, 10));
                    $("select[name=shour]").val(list[0].sdate.substr(11, 2));
                    $("select[name=sMin]").val(list[0].sdate.substr(14, 2));
                    $("input:text[name=edate]").val(list[0].edate.substr(0, 10));
                    $("select[name=ehour]").val(list[0].edate.substr(11, 2));
                    $("select[name=eMin]").val(list[0].edate.substr(14, 2));
                    $("input:text[name=title]").val(list[0].Title);
                    for (var ix = 0; ix < list.length; ix++) {
                        $("select[name=" + list[ix].ConstDefine + "]").val(list[ix].value);
                    }

                    $('#deleteButton').html('<button type="button" class="btn"  onclick="deleteCheck(\'' + list[0].flrag + '\',\'end\')">종료</button> <button type="button" class="btn"  onclick="deleteCheck(\'' + list[0].flrag + '\',\'del\')">삭제</button>');
                }
            }
    });
    openLayer2('noticePopup', { top: 200 }, 1);
}

function RubyEventView(idx) {
    var urllink = "../event/rubyEvent_view.asp?idx=" + idx;

    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                var spanID;
                var basicValue ;
                if (list.length > 0) {

                    $("input:hidden[name=idx]").val(list[0].idx);
                    $("input:text[name=sdate]").val(list[0].sdate.substr(0, 10));
                    $("select[name=shour]").val(list[0].sdate.substr(11, 2));
                    $("select[name=sMin]").val(list[0].sdate.substr(14, 2));
                    $("input:text[name=edate]").val(list[0].edate.substr(0, 10));
                    $("select[name=ehour]").val(list[0].edate.substr(11, 2));
                    $("select[name=eMin]").val(list[0].edate.substr(14, 2));
                    $("input:text[name=title]").val(list[0].Title);
                    for (var ix = 0; ix < list.length; ix++) {
                        $("input:text[name=" + list[ix].ConstDefine + "]").val(list[ix].value);
                        spanID = list[ix].ConstDefine.replace("DEF_SHOP_INC_RUBY_", "").replace("_AMOUNT", "");
                        basicValue = $('#ruby' + spanID).html();
                        $('#ruby' + spanID).html(Number(basicValue) + list[ix].value);
                    }

                    $('#deleteButton').html('<button type="button" class="btn"  onclick="deleteCheck(\'' + list[0].flrag + '\',\'end\')">종료</button> <button type="button" class="btn"  onclick="deleteCheck(\'' + list[0].flrag + '\',\'del\')">삭제</button>');
                }
            }
    });
    openLayer2('noticePopup', { top: 200 }, 1);
}

function dungeonView(idx) {
    var urllink = "../event/dungeon_view.asp?idx=" + idx;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {

                $("input:hidden[name=idx]").val($(data).attr("idx"));
                $("input:radio[name=div]:input[value=" + $(data).attr("div") + "]").attr("checked", true);
                if ($(data).attr("div") == 0) {
                    $("input:text[name=sdate]").attr("disabled", true);
                    $("input:text[name=edate]").attr("disabled", true);
                }
                else {
                    $("input:text[name=sdate]").removeAttr("disabled");
                    $("input:text[name=edate]").removeAttr("disabled");
                    $("input:text[name=sdate]").val($(data).attr("startdate").substr(0, 10));
                    $("input:text[name=edate]").val($(data).attr("enddate").substr(0, 10));
                }
                $("select[name=shour]").val($(data).attr("startdate").substr(11, 2));
                $("select[name=sMin]").val($(data).attr("startdate").substr(14, 2));
                $("select[name=ehour]").val($(data).attr("enddate").substr(11, 2));
                $("select[name=eMin]").val($(data).attr("enddate").substr(14, 2));
                $('#deleteButton').html(' <button type="button" class="btn"  onclick="deleteCheck()">삭제</button>');
            }
    });
    openLayer('noticePopup', { top: 200 });
}

function guildWarOpenTimeView(idx) {
    var urllink = "../event/guildwar_view.asp?idx=" + idx;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {

                $("input:hidden[name=idx]").val($(data).attr("idx"));
                $("input:radio[name=type]:input[value=" + $(data).attr("type") + "]").attr("checked", true);
                $("select[name=shour]").val($(data).attr("startsec"));
                $("select[name=ehour]").val($(data).attr("endsec"));
                $('#deleteButton').html(' <button type="button" class="btn"  onclick="deleteCheck()">삭제</button>');
            }
    });
    openLayer('noticePopup', { top: 200 });
}

function rubyPVPOpenTimeView(idx) {
    var urllink = "../event/rubypvptime_view.asp?idx=" + idx;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {

                $("input:hidden[name=idx]").val($(data).attr("idx"));
                //$("input:radio[name=type]:input[value=" + $(data).attr("type") + "]").attr("checked", true);
                $("select[name=shour]").val($(data).attr("startsec"));
                $("select[name=ehour]").val($(data).attr("endsec"));
                $('#deleteButton').html(' <button type="button" class="btn"  onclick="deleteCheck()">삭제</button>');
            }
    });
    openLayer('noticePopup', { top: 200 });
}

function loginEventView(idx) {
    ChekNullIdx('noticePop', 'idx');
    var urllink = "../event/loginEvent_view.asp?idx=" + idx;

    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {
                    $("input:hidden[name=idx]").val(list[0].idx);
                    $("input:radio[name=applytype]:input[value=" + list[0].ApplyType + "]").attr("checked", true);
                    if (list[0].ApplyType == "2") {
                        $("input:text[name=applydate]").removeAttr("disabled");
                        $("select[name=applyhour]").removeAttr("disabled");
                        $("select[name=applyMin]").removeAttr("disabled");
                        $("input:text[name=applydate]").val(list[0].ApplyDate.substr(0, 10));
                        $("select[name=applyhour]").val(list[0].ApplyDate.substr(11, 2));
                        $("select[name=applyMin]").val(list[0].ApplyDate.substr(14, 2));
                    }
                    else{
                        $("input:text[name=applydate]").attr("disabled", true);
                        $("select[name=applyhour]").attr("disabled", true);
                        $("select[name=applyMin]").attr("disabled", true);
                    }
                    $("input:text[name=sdate]").val(list[0].ReceiveSDate.substr(0, 10));
                    $("select[name=shour]").val(list[0].ReceiveSDate.substr(11, 2));
                    $("select[name=sMin]").val(list[0].ReceiveSDate.substr(14, 2));
                    $("input:text[name=edate]").val(list[0].ReceiveEDate.substr(0, 10));
                    $("select[name=ehour]").val(list[0].ReceiveEDate.substr(11, 2));
                    $("select[name=eMin]").val(list[0].ReceiveEDate.substr(14, 2));
                    for (var ix = 0; ix < list.length; ix++) {
                        $("input:text[name=item" + list[ix].itemID + "]").val(list[ix].itema);
                    }
                }
            }
    });
    openLayer2('noticePopup', { top: 200 }, 1);
}

function eventNoticeView(idx) {
    var urllink = "../notice/Notice_view.asp?idx=" + idx;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                document.noticePop.idx.value = $(data).attr("idx");
                document.noticePop.sdate.value = $(data).attr("sdate").substr(0, 10);
                document.noticePop.edate.value = $(data).attr("edate").substr(0, 10);
                $("select[name=shour]").val($(data).attr("sdate").substr(11, 2));
                $("select[name=sMin]").val($(data).attr("sdate").substr(14, 2));
                $("select[name=ehour]").val($(data).attr("edate").substr(11, 2));
                $("select[name=eMin]").val($(data).attr("edate").substr(14, 2));
                $("input:radio[name=type]:input[value=" + $(data).attr("type") + "]").attr("checked", true);
                if ($(data).attr("type") == 1) {
                    $("#contents").attr("disabled", true);
                }
                else {
                    $("#contents").removeAttr("disabled");
                }

                $("input:text[name=imgFile]").val($(data).attr("imgFile"));
                $("input:text[name=link]").val($(data).attr("link"));
                $("input:text[name=orderNum]").val($(data).attr("orderNum"));
                $("textarea[name=contents]").val($(data).attr("contents"));
            }
    });
    openLayer2('noticePopup', { top: 200 });
}

function lineNoticeView(idx) {
    $("form").each(function () {
        if (this.id == "noticePop") this.reset();
    });
    var urllink = "../notice/lineNotice_view.asp?idx=" + idx;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {
                    document.noticePop.groupID.value = list[0].groupID;
                    document.noticePop.sdate.value = list[0].sdate.substr(0, 10);
                    document.noticePop.edate.value = list[0].edate.substr(0, 10);
                    $("select[name=shour]").val(list[0].sdate.substr(11, 2));
                    $("select[name=sMin]").val(list[0].sdate.substr(14, 2));
                    $("select[name=ehour]").val(list[0].edate.substr(11, 2));
                    $("select[name=eMin]").val(list[0].edate.substr(14, 2));
                    $("input:radio[name=displaytype]:input[value=" + list[0].type + "]").attr("checked", true);
                    if ($(data).attr("displayTime") > 0) {
                        $("input:text[name=displayTime]").removeAttr("disabled");
                        $("input:text[name=displayTime]").val(list[0].displayTime);
                    }
                    for (var i = 0; i < list.length; i++) {
                        if (list[i].languagecode == 0) {
                            $("textarea[name=contents_kr]").val(list[i].contents);
                            $('#limit_kr').html('(' + (60 - $('textarea[name=contents_kr]').val().length) + ' / 60)');
                        }
                        else if (list[i].languagecode == 1) {
                            $("textarea[name=contents_en]").val(list[i].contents);
                            $('#limit_en').html('(' + (60 - $('textarea[name=contents_en]').val().length) + ' / 60)');
                        }
                        else if (list[i].languagecode == 2) {
                            $("textarea[name=contents_jp]").val(list[i].contents);
                            $('#limit_jp').html('(' + (60 - $('textarea[name=contents_jp]').val().length) + ' / 60)');
                        }
                        else if (list[i].languagecode == 3) {
                            $("textarea[name=contents_cns]").val(list[i].contents);
                            $('#limit_cns').html('(' + (60 - $('textarea[name=contents_cns]').val().length) + ' / 60)');
                        }
                        else if (list[i].languagecode == 4) {
                            $("textarea[name=contents_cnt]").val(list[i].contents);
                            $('#limit_cnt').html('(' + (60 - $('textarea[name=contents_cnt]').val().length) + ' / 60)');
                        }
                        else if (list[i].languagecode == 5) {
                            $("textarea[name=contents_spn]").val(list[i].contents);
                            $('#limit_spn').html('(' + (60 - $('textarea[name=contents_spn]').val().length) + ' / 60)');
                        }
                    }
                }
                $("#buttonGubun").html('<button type="button" class="btn" onclick="noticeDel()">삭제</button>')
            }
    });
    openLayer2('noticePopup', { top: 200 }, 1);
}

function pupupNoticeView(idx) {

    var urllink = "../notice/popupNotice_view.asp?groupID=" + idx;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {
                    $("input:hidden[name=idx]").val(list[0].groupID);
                    $("input:text[name=sdate]").val(list[0].sdate.substr(0, 10));
                    $("input:text[name=edate]").val(list[0].edate.substr(0, 10));
                    $("select[name=shour]").val(list[0].sdate.substr(11, 2));
                    $("select[name=sMin]").val(list[0].sdate.substr(14, 2));
                    $("select[name=ehour]").val(list[0].edate.substr(11, 2));
                    $("select[name=eMin]").val(list[0].edate.substr(14, 2));
                    $("input:text[name=orderNum]").val(list[0].orderNum);

                    for (var i = 0; i < list.length; i++) {
                        if (list[i].languagecode == 0) {
                            $("input:text[name=imgUrl_kr]").val(list[i].imgUrl);
                            $("input:text[name=link_kr]").val(list[i].linkUrl);
                        }
                        else if (list[i].languagecode == 1) {
                            $("input:text[name=imgUrl_en]").val(list[i].imgUrl);
                            $("input:text[name=link_en]").val(list[i].linkUrl);
                        }
                        else if (list[i].languagecode == 2) {
                            $("input:text[name=imgUrl_jp]").val(list[i].imgUrl);
                            $("input:text[name=link_jp]").val(list[i].linkUrl);
                        }
                        else if (list[i].languagecode == 3) {
                            $("input:text[name=imgUrl_cns]").val(list[i].imgUrl);
                            $("input:text[name=link_cns]").val(list[i].linkUrl);
                        }
                        else if (list[i].languagecode == 4) {
                            $("input:text[name=imgUrl_cnt]").val(list[i].imgUrl);
                            $("input:text[name=link_cnt]").val(list[i].linkUrl);
                        }
                        else if (list[i].languagecode == 5) {
                            $("input:text[name=imgUrl_spn]").val(list[i].imgUrl);
                            $("input:text[name=link_spn]").val(list[i].linkUrl);
                        }
                    }
                }


            }
    });
    openLayer2('noticePopup', { top: 200 }, 1);
}
function itemSelect() {
    $(".item").on("click", function () {
        var clickedRow = $(this).parent().parent().index();
        //alert(clickedRow);
        var item_type = $("select[name=item_type]").eq(clickedRow).val();
        var item_class = $("select[name=item_class]").eq(clickedRow).val();
        var item_grade = $("select[name=item_grade]").eq(clickedRow).val();
        var item_tier = $("select[name=item_tier]").eq(clickedRow).val();
        var urllink = "../gameinfo/item_option.asp?itemType=" + item_type + "&classType=" + item_class + "&tier=" + item_tier + "&grade=" + item_grade;
        $.ajax({
            url: urllink
                , dataType: "json"
                , success: function (data) {
                    var list = $(data);
                    if (list.length > 0) {
                        for (var ix = 1; ix <= 5; ix++) {
                            $("select[name=item_option" + ix + "]").eq(clickedRow).find('option').each(function () {
                                $(this).remove();
                            });
                            $("select[name=optionMin" + ix + "]").eq(clickedRow).find('option').each(function () {
                                $(this).remove();
                            });
                            $("select[name=optionMax" + ix + "]").eq(clickedRow).find('option').each(function () {
                                $(this).remove();
                            });
                            $("input:text[name=item_value" + ix + "]").eq(clickedRow).val("");
                            $("select[name=item_option" + ix + "]").eq(clickedRow).append("<option value=''>선택</option>");
                            $("select[name=optionMin" + ix + "]").eq(clickedRow).append("<option value=''>선택</option>");
                            $("select[name=optionMax" + ix + "]").eq(clickedRow).append("<option value=''>선택</option>");
                            for (var i = 0; i < list.length; i++) {
                                $("select[name=item_option" + ix + "]").eq(clickedRow).append("<option value='" + list[i].ParameterType + "'>" + getOptionType(list[i].ParameterType) + "</option>");
                                $("select[name=optionMin" + ix + "]").eq(clickedRow).append("<option value='" + list[i].OptionValue1 + "'>" + list[i].OptionValue1 + "</option>");
                                $("select[name=optionMax" + ix + "]").eq(clickedRow).append("<option value='" + list[i].OptionValue2 + "'>" + list[i].OptionValue2 + "</option>");
                            }
                        }
                    }
                }
        });
    });
}

function optionSelect(option) {
    var clickedRow = $(option).parent().parent().index();
    var index = $(option).children("option:selected").index();
    var name = $(option).attr("name");
    if (name == "item_option1") {
        $("input:text[name=item_value1]").eq(clickedRow).val("");
        $("select[name=optionMin1]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
        $("select[name=optionMax1]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
    }
    else if (name == "item_option2") {
        $("input:text[name=item_value2]").eq(clickedRow).val("");
        $("select[name=optionMin2]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
        $("select[name=optionMax2]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
    }
    else if (name == "item_option3") {
        $("input:text[name=item_value3]").eq(clickedRow).val("");
        $("select[name=optionMin3]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
        $("select[name=optionMax3]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
    }
    else if (name == "item_option4") {
        $("input:text[name=item_value4]").eq(clickedRow).val("");
        $("select[name=optionMin4]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
        $("select[name=optionMax4]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
    }
    else if (name == "item_option5") {
        $("input:text[name=item_value5]").eq(clickedRow).val("");
        $("select[name=optionMin5]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
        $("select[name=optionMax5]").eq(clickedRow).children("option:eq(" + index + ")").attr("selected", "selected");
    }
}

function valueCheck() {
    $(".valueCheck").bind("blur", function (event) {
        var clickedRow = $(this).parent().parent().index();
        var name = $(this).attr("name");
        var min;
        var max;
        if (name == "item_value1") {
            if ($("select[name=item_option1]").eq(clickedRow).val() == "") {
                min = 0;
                max = 0;
            }
            else {
                min = $("select[name=optionMin1]").eq(clickedRow).children("option:selected").val();
                max = $("select[name=optionMax1]").eq(clickedRow).children("option:selected").val();
            }
        }
        else if (name == "item_value2") {
            if ($("select[name=item_option2]").eq(clickedRow).val() == "") {
                min = 0;
                max = 0;
            }
            else {
                min = $("select[name=optionMin2]").eq(clickedRow).children("option:selected").val();
                max = $("select[name=optionMax2]").eq(clickedRow).children("option:selected").val();
            }
        }
        else if (name == "item_value3") {
            if ($("select[name=item_option3]").eq(clickedRow).val() == "") {
                min = 0;
                max = 0;
            }
            else {
                min = $("select[name=optionMin3]").eq(clickedRow).children("option:selected").val();
                max = $("select[name=optionMax3]").eq(clickedRow).children("option:selected").val();
            }
        }
        else if (name == "item_value4") {
            if ($("select[name=item_option4]").eq(clickedRow).val() == "") {
                min = 0;
                max = 0;
            }
            else {
                min = $("select[name=optionMin4]").eq(clickedRow).children("option:selected").val();
                max = $("select[name=optionMax4]").eq(clickedRow).children("option:selected").val();
            }
        }
        else if (name == "item_value5") {
            if ($("select[name=item_option5]").eq(clickedRow).val() == "") {
                min = 0;
                max = 0;
            }
            else {
                min = $("select[name=optionMin5]").eq(clickedRow).children("option:selected").val();
                max = $("select[name=optionMax5]").eq(clickedRow).children("option:selected").val();
            }
        }
        if ($(this).val() != "") {
            if (max < parseInt($(this).val())) {
                alert("최대 " + max + " 까지 입력가능합니다.");
                $(this).val("");
                $(this).focus();
            }
            if (min > parseInt($(this).val())) {
                alert("최소 " + min + " 까지 입력가능합니다.");
                $(this).val("");
                $(this).focus();
            }
        }
    });
}

function searchitme(gubun) {
    if (gubun == 0) {
        $(".searchSoul").on("click", function () {
            var clickedRow = $(this).parent().parent().index();
            var classtype = $("select[name=soul_class]").eq(clickedRow).val();
            $("select[name=soulclass]").val($("select[name=soul_class]").eq(clickedRow).val()).attr("selected", "selected");
            if (classtype == "") {
                alert("클래스를 선택하세요.");
            } else {
                $("input:hidden[name=soulIndex]").val(clickedRow);
                var searchValue = $("input[name=soul_name]:eq(" + clickedRow + ")").val();
                var urllink = "../gameinfo/soul_search.asp?" + encodeURI("classType=" + classtype + "&name=" + searchValue);
                $.ajax({
                    url: urllink
						, contentType: 'text/html; charset=utf-8'
                        , dataType: "json"

                        , success: function (data) {
                            var list = $(data);
                            var innerhtml = "";
                            if (list.length > 0) {
                                innerhtml = "<table class='table table-bordered'><tbody><tr><th>혼이름</th><th>등급</th><th></th></tr>";
                                for (var i = 0; i < list.length; i++) {
                                    innerhtml = innerhtml + "<tr><td>" + list[i].String + "</td><td>" + list[i].grade + "</td><td><button type='button' class='btn' onclick='setSoul(" + list[i].SoulID + ",\"" + list[i].String + "\"," + list[i].grade + "," + clickedRow + ");'>선택</button></td></tr>";
                                }
                                innerhtml = innerhtml + "</tbody></table>";
                                $("#list").html(innerhtml);
                                openLayer('soulList', { top: 300 });
                            }
                        }
                });
            }
        });
    }
    else {
        var clickedRow = $("input:hidden[name=soulIndex]").val();
        var classtype = $("select[name=soulclass]").val();
        if (classtype == "") {
            alert("클래스를 선택하세요.");
        } else {
            var searchValue = $("input[name=soulname]").val();
            var urllink = "soul_search.asp?" + encodeURI("classType=" + classtype + "&name=" + searchValue);
            $.ajax({
                url: urllink
						, contentType: 'text/html; charset=utf-8'
                        , dataType: "json"
                        , success: function (data) {
                            var list = $(data);
                            var innerhtml = "";
                            if (list.length > 0) {
                                innerhtml = "<table class='table table-bordered'><tbody><tr><th>혼이름</th><th>등급</th><th></th></tr>";
                                for (var i = 0; i < list.length; i++) {
                                    innerhtml = innerhtml + "<tr><td>" + list[i].String + "</td><td>" + list[i].grade + "</td><td><button type='button' class='btn' onclick='setSoul(" + list[i].SoulID + ",\"" + list[i].String + "\"," + list[i].grade + "," + clickedRow + ");'>선택</button></td></tr>";
                                }
                                innerhtml = innerhtml + "</tbody></table>";
                                $("#list").html(innerhtml);
                                openLayer('soulList', { top: 300 });
                            }
                        }
            });
        }
    }
}
function searchBuff(me, gubunType) {
    var clickedRow = $(me).parent().parent().index();
    var classtype = $("select[name=soul_class]").eq(clickedRow).val();
    var name = $("input[name=soul_name]:eq(" + clickedRow + ")").val();
    var grade = $("select[name=soul_grade]").eq(clickedRow).val();
    var soulid = $("input[name=soul_id]:eq(" + clickedRow + ")").val();

    if (classtype == "") {
        alert("클래스를 선택하세요.");
        return false;
    }
    if (name == "") {
        alert("이름을 입력하세요.");
        return false;
    }
    if (grade == "") {
        alert("등급을 선택하세요.");
        return false;
    }
    var urllink = "soul_buff.asp?" + encodeURI("classType=" + classtype + "&name=" + name + "&soulid=" + soulid + "&grade=" + grade + "&groupType=" + gubunType + "&row=" + clickedRow);
    window.open(urllink, "soulBuff", "scrollbars=auto, resizable=yes, top=500, left=500, width=600, height=400");
}
function setSoul(id, name, grade, row) {
    var $layer = $("#soulList");
    $("select[name=soul_class]").eq(row).val($("select[name=soulclass]").val()).attr("selected", "selected");
    $("input:hidden[name=soul_id]").eq(row).val(id);
    $("input:text[name=soul_name]").eq(row).val(name);
    $("select[name=soul_grade]").eq(row).val(grade).attr("selected", "selected");
    $("input:hidden[name=buffid1]").eq(row).val("");
    $("textarea[name=buffStr1]").eq(row).val("");
    $("textarea[name=buffStr1]").eq(row).hide();
    $("input:hidden[name=buffid2]").eq(row).val("");
    $("textarea[name=buffStr2]").eq(row).val("");
    $("textarea[name=buffStr2]").eq(row).hide();
    if ($layer.is(':visible')) {
        $layer.hide();
    }
}

function setBuff(id, buff, gubunType, row) {
    if (gubunType == "1") {
        $("input:hidden[name=buffid1]").eq(row).val(id);
        $("textarea[name=buffStr1]").eq(row).val(buff);
        $("textarea[name=buffStr1]").eq(row).show();
    }
    else {
        $("input:hidden[name=buffid2]").eq(row).val(id);
        $("textarea[name=buffStr2]").eq(row).val(buff);
        $("textarea[name=buffStr2]").eq(row).show();
    }
}

function itemView(aid, idx) {
    if ($("#popupView").is(':visible')) {
        $("#popupView").hide();
    }
    var urllink = "../gameinfo/item_view.asp?aid=" + aid + "&idx=" + idx;
    var info = ""
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                info = "<h4>" + $(data).attr("name") + "</h4><table class='table table-bordered'><tbody><tr>";
                info = info + "<th>차수</th><td>" + $(data).attr("Tier") + "</td>";
                info = info + "<th>등급</th><td>" + $(data).attr("enchant_grade") + "</td>";
                info = info + "<th>강화</th><td>" + $(data).attr("enchant_level") + "</td></tr>";
                info = info + "<tr><th>옵션</th><td colspan='5'>";
                if ($(data).attr("MainParameter_Value1") != "" && $(data).attr("MainParameter_Value2") == "") {
                    info = info + "방어력 : " + $(data).attr("MainParameter_Value1") + "<br />";
                }
                if ($(data).attr("MainParameter_Value2") != "") {
                    info = info + "공격력 : " + $(data).attr("MainParameter_Value2") + "<br />";
                }
                if ($(data).attr("optiontype1") != "" && $(data).attr("optionvalue1") > 0) {
                    if ($(data).attr("optiontype1").indexOf("RATE") == -1) {
                        info = info + getOptionType($(data).attr("optiontype1")) + " : " + $(data).attr("optionvalue1") + "<br />";
                    }
                    else {
                        info = info + getOptionType($(data).attr("optiontype1")) + " : " + $(data).attr("optionvalue1") + "%<br />";
                    }
                }
                if ($(data).attr("optiontype2") != "" && $(data).attr("optionvalue2") > 0) {
                    if ($(data).attr("optiontype2").indexOf("RATE") == -1) {
                        info = info + getOptionType($(data).attr("optiontype2")) + " : " + $(data).attr("optionvalue2") + "<br />";
                    }
                    else {
                        info = info + getOptionType($(data).attr("optiontype2")) + " : " + $(data).attr("optionvalue2") + "%<br />";
                    }
                }
                if ($(data).attr("optiontype3") != "" && $(data).attr("optionvalue3") > 0) {
                    if ($(data).attr("optiontype3").indexOf("RATE") == -1) {
                        info = info + getOptionType($(data).attr("optiontype3")) + " : " + $(data).attr("optionvalue3") + "<br />";
                    }
                    else {
                        info = info + getOptionType($(data).attr("optiontype3")) + " : " + $(data).attr("optionvalue3") + "%<br />";
                    }
                }
                if ($(data).attr("optiontype4") != "" && $(data).attr("optionvalue4") > 0) {
                    if ($(data).attr("optiontype4").indexOf("RATE") == -1) {
                        info = info + getOptionType($(data).attr("optiontype4")) + " : " + $(data).attr("optionvalue4") + "<br />";
                    }
                    else {
                        info = info + getOptionType($(data).attr("optiontype4")) + " : " + $(data).attr("optionvalue4") + "%<br />";
                    }

                }
                if ($(data).attr("optiontype5") != "" && $(data).attr("optionvalue5") > 0) {
                    if ($(data).attr("optiontype5").indexOf("RATE") == -1) {
                        info = info + getOptionType($(data).attr("optiontype5")) + " : " + $(data).attr("optionvalue5") + "<br />";
                    }
                    else {
                        info = info + getOptionType($(data).attr("optiontype5")) + " : " + $(data).attr("optionvalue5") + "%<br />";
                    }
                }
                info = info + "</td></tr></tbody></table>";

                $('#Item_info').html(info);
            }
    });
    openLayer('popupView', { top: 200 });
}

function itemLogView(idx) {
    var maxCnt = $(".popupView").length;
    for (var i = 0; i < maxCnt; i++) {
        if ($(".popupView").is(':visible')) {
            $(".popupView").hide();
        }
    }
    openLayer('popupView' + idx, { top: 200 });
}

function soulView(aid, idx) {
    if ($("#popupView").is(':visible')) {
        $("#popupView").hide();
    }
    var urllink = "../gameinfo/soul_view.asp?aid=" + aid + "&idx=" + idx;
    var info = "";
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var mptype = "";
                info = "<h4> " + $(data).attr("name") + "</h4><table class='table table-bordered'><tbody><tr>";
                info = info + "<th>등급</th><td>" + $(data).attr("Grade") + "</td>";
                info = info + "<th>레벨</th><td>" + $(data).attr("level") + "</td></tr>";
                info = info + "<th>레벨경험치</th><td>" + $(data).attr("levelup_kill_count") + " / " + $(data).attr("Max_Kill_Count") + "</td>";
                info = info + "<th>합성경험치</th><td>" + $(data).attr("upgrade_gauge") + " / " + $(data).attr("Max_Gauge") + "</td></tr>";
                if ($(data).attr("MPType") == "MPConsume") {
                    mptype = "-";
                }
                else if ($(data).attr("MPType") == "MPRecovery") {
                    mptype = "+";
                }
                info = info + "<tr><th>쿨타임</th><td>" + $(data).attr("Cooltime") + "</td>";
                info = info + "<th>MP소모</th><td>" + mptype + $(data).attr("MPValue") + "</td></tr>";
                info = info + "<tr><th>스킬명</th><td>" + $(data).attr("skillName") + "</td>";
                info = info + "<th>스킬타입</th><td>" + $(data).attr("Active_Or_Passive") + "</td></tr>";
                info = info + "<tr><th>스킬 설명</th><td colspan='3'>" + $(data).attr("skillDes") + "</td></tr>";
                info = info + "<tr><th>스킬 특수 능력1</th><td colspan='3'>" + $(data).attr("buff1") + "</td></tr>";
                info = info + "<tr><th>스킬 특수 능력2</th><td colspan='3'>" + $(data).attr("buff2") + "</td></tr>";
                info = info + "<tr><th>유니크 스킬</th><td colspan='3'>" + $(data).attr("buff3") + "</td></tr>";

                info = info + "</td></tr></tbody></table>";

                $('#soul_info').html(info);
            }
    });
    openLayer('popupView', { top: 200 });
}

function soulLogView(aid, masterid, soulid, sooulSeq, tablename) {
    if ($("#popupView").is(':visible')) {
        $("#popupView").hide();
    }
    var urllink = "../loginfo/soul_logview.asp?soulid=" + soulid + "&aid=" + aid + "&masterid=" + masterid + "&sooulSeq=" + sooulSeq + "&tablename=" + tablename;
    alert(urllink);
    var info = "";
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var mptype = "";
                info = "<h4> " + $(data).attr("name") + "</h4><table class='table table-bordered'><tbody><tr>";
                info = info + "<th>등급</th><td>" + $(data).attr("grade") + "</td>";
                info = info + "<th>레벨</th><td>" + $(data).attr("level") + "</td></tr>";
                info = info + "<th>레벨경험치</th><td>" + $(data).attr("LevelUP_KILL_COUNT") + " / " + $(data).attr("Max_Kill_Count") + "</td>";
                info = info + "<th>합성경험치</th><td>" + $(data).attr("Upgrade_Gauge") + " / " + $(data).attr("Max_Gauge") + "</td></tr>";
                if ($(data).attr("MPType") == "MPConsume") {
                    mptype = "-";
                }
                else if ($(data).attr("MPType") == "MPRecovery") {
                    mptype = "+";
                }
                info = info + "<tr><th>쿨타임</th><td>" + $(data).attr("Cooltime") + "</td>";
                info = info + "<th>MP소모</th><td>" + mptype + $(data).attr("MPValue") + "</td></tr>";
                info = info + "<tr><th>스킬명</th><td>" + $(data).attr("skillName") + "</td>";
                info = info + "<th>스킬타입</th><td>" + $(data).attr("Active_Or_Passive") + "</td></tr>";
                info = info + "<tr><th>스킬 설명</th><td colspan='3'>" + $(data).attr("skillDes") + "</td></tr>";
                info = info + "<tr><th>스킬 특수 능력1</th><td colspan='3'>" + $(data).attr("buff1") + "</td></tr>";
                info = info + "<tr><th>스킬 특수 능력2</th><td colspan='3'>" + $(data).attr("buff2") + "</td></tr>";
                info = info + "<tr><th>유니크 스킬</th><td colspan='3'>" + $(data).attr("buff3") + "</td></tr>";

                info = info + "</td></tr></tbody></table>";

                $('#soul_info').html(info);
            }
    });
    openLayer('popupView', { top: 200 });
}

function openLayer(targetID, options) {
    var $layer = $('#' + targetID);
    var $close = $layer.find('.popClose');
    var width = $layer.outerWidth();
    var ypos = options.top;
    var xpos = options.left;
    var marginLeft = 0;

    if (xpos == undefined) {
        xpos = '50%';
        marginLeft = -(width / 2);
    }

    if (!$layer.is(':visible')) {
        $layer.css({ 'top': ypos + 'px', 'left': xpos, 'margin-left': marginLeft }).show();
    }
    $close.bind('click', function () {
        $(".dlsplayHidden").hide();
        if ($layer.is(':visible')) {
            $layer.hide();
            $("form").each(function () {
                if (this.id == "noticePop") this.reset();
            });
        }
        return false;
    });
}

function tableAdd(table) {
    // clone
    var index = 0;
    $("#" + table + " > tbody").children('tr').each(function () {
        index = index + 1
    });
    if (index < 10) {
        if (table == "itemadd") {
            $.trClone = '<td><select name="item_class" class="item" style="width:100px;" onchange="itemSelect()"><option value="">선택</option><option value="1">워리어</option><option value="2">소드마스터</option></select></td><td><select name="item_type" class="item" style="width:80px" onchange="itemSelect()"><option value="">선택</option><option value="ItemType_Helmet">투구</option><option value="ItemType_Armor">갑옷</option><option value="ItemType_Glove">장갑</option><option value="ItemType_Shoes">신발</option><option value="ItemType_GSword">대도</option><option value="ItemType_DSword">쌍검</option><option value="ItemType_Accessory">장신구</option></select></td>';
            $.trClone = $.trClone + '<td><select name="item_tier" class="item" style="width:65px" onchange="itemSelect()"><option value="">선택</option><option value="0">0차</option><option value="1">1차</option><option value="2">2차</option><option value="3">3차</option><option value="4">4차</option><option value="5">5차</option><option value="6">6차</option><option value="7">7차</option></select></td>';
            $.trClone = $.trClone + '<td><select name="item_grade" class="item" style="width:65px" onchange="itemSelect()"><option value="">선택</option><option value="1">1성</option><option value="2">2성</option><option value="3">3성</option><option value="4">4성</option><option value="5">5성</option></select></td>';
            $.trClone = $.trClone + '<td><select name="item_level" style="width:65px"><option value="">선택</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option></select></td>';
            $.trClone = $.trClone + '<td><input type="text"  name="item_exp" style="width:40px" /></td>';
            $.trClone = $.trClone + '<td><select name="optionMin1" class="dlsplayHidden"><option value=""></option></select><select name="optionMax1" class="dlsplayHidden"><option value=""></option></select><select name="item_option1" style="width:100px" onchange="optionSelect(this)"><option value="">선택</option></select>&nbsp;<input type="text" class="onlyNumber valueCheck" onfocus="onlyNumber();valueCheck();" name="item_value1" style="width:20px;" /></td>';
            $.trClone = $.trClone + '<td><select name="optionMin2" class="dlsplayHidden"><option value=""></option></select><select name="optionMax2" class="dlsplayHidden"><option value=""></option></select><select name="item_option2" style="width:100px" onchange="optionSelect(this)"><option value="">선택</option></select>&nbsp;<input type="text" class="onlyNumber valueCheck" onfocus="onlyNumber();valueCheck();" name="item_value2" style="width:20px;" /></td>';
            $.trClone = $.trClone + '<td><select name="optionMin3" class="dlsplayHidden"><option value=""></option></select><select name="optionMax3" class="dlsplayHidden"><option value=""></option></select><select name="item_option3" style="width:100px" onchange="optionSelect(this)"><option value="">선택</option></select>&nbsp;<input type="text" class="onlyNumber valueCheck" onfocus="onlyNumber();valueCheck();" name="item_value3" style="width:20px;" /></td>';
            $.trClone = $.trClone + '<td class="dlsplayHidden"><select name="optionMin4" class="dlsplayHidden"><option value=""></option></select><select name="optionMax4" class="dlsplayHidden"><option value=""></option></select><select name="item_option4" style="width:100px" onchange="optionSelect(this)"><option value="">선택</option></select>&nbsp;<input type="text" class="onlyNumber valueCheck" onfocus="onlyNumber();valueCheck();" name="item_value4" style="width:20px;" /></td>';
            $.trClone = $.trClone + '<td class="dlsplayHidden"><select name="optionMin5" class="dlsplayHidden"><option value=""></option></select><select name="optionMax5" class="dlsplayHidden"><option value=""></option></select><select name="item_option5" style="width:100px" onchange="optionSelect(this)"><option value="">선택</option></select>&nbsp;<input type="text" class="onlyNumber valueCheck" onfocus="onlyNumber();valueCheck();" name="item_value5" style="width:20px;" /></td>';
        }
        else if (table == "souladd") {
            $.trClone = '<td><select name="soul_class" style="width:100px;"><option value="">선택</option><option value="all">공용</option><option value="pc1">워리어</option><option value="pc2">소드마스터</option></select></td>';
            $.trClone = $.trClone + '<td><input type="text" name="soul_name" style="width:80px;" /> <button type="button" class="btn searchSoul" onclick="searchitme(0);">검색</button></td>';
            $.trClone = $.trClone + '<td><input type="hidden" name="soul_id" /><select name="soul_grade" style="width:60px"><option value="1">1성</option><option value="2">2성</option><option value="3">3성</option><option value="4">4성</option><option value="5">5성</option></select></td>';
            $.trClone = $.trClone + '<td><select name="soul_level" style="width:60px"><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option></select></td>';
            $.trClone = $.trClone + '<td><input type="text" name="soul_exp" style="width:60px" /></td>';
            $.trClone = $.trClone + '<td><input type="hidden" name="buffid1" /><textarea name="buffStr1" class="dlsplayHidden"></textarea><button type="button" class="btn" onclick="searchBuff(this,1)">스킬선택</button></td>';
            $.trClone = $.trClone + '<td><input type="hidden" name="buffid2" /><textarea name="buffStr2" class="dlsplayHidden"></textarea><button type="button" class="btn" onclick="searchBuff(this,2)">스킬선택</button></td>';
        }
        else {
            $.trClone = $("#" + table + " tr:last").clone().html();
        }
        $.newTr = $("<tr>" + $.trClone + "</tr>");
        // append
        $("#" + table + "").append($.newTr);
    }
    else {
        alert("최대 10개까지 지급가능합니다.");
    }
}

function getRead(data) {
    var stringArray = data.split("\n");
    var usercnt = stringArray.length;
    if ($.trim(stringArray[0].toUpperCase()) == "SNO") {
        usercnt = usercnt - 1;
        data = data.replace(stringArray[0] + "\n", "");
        $("input:hidden[name=usertype]").val("SNO");
    }
    else if ($.trim(stringArray[0].toUpperCase()) == "별명" || $.trim(stringArray[0].toUpperCase()) == "닉네임" || $.trim(stringArray[0].toUpperCase()) == "USERNAME") {
        usercnt = usercnt - 1;
        data = data.replace(stringArray[0] + "\n", "");
        $("input:hidden[name=usertype]").val("UserName");
    }
    else {
        alert("첫 줄에는 SNO, 별명, 닉네임, UserName 중 하나로 입력하세요.");
        return false;
    }
    if (usercnt > 500) {
        alert("대량 지급은 최대 500명까지 가능합니다.");
        return false;
    }
    $("textarea[name=user]").val(data);
    $("input:hidden[name=userCnt]").val(usercnt);
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        var encoding = 'UTF-8';
        reader.onload = function (e) {
            getRead(reader.result);
        }
        //reader.readAsDataURL();
        reader.readAsText(input.files[0], encoding);
    }
}

function couponTypeSelect(couponType) {
    if (couponType == 0) {
        $("select[name=reward2]").removeAttr("disabled");
        $("select[name=reward3]").removeAttr("disabled");
    }
    else {
        $("select[name=reward2]").attr("disabled", true);
        $("select[name=reward3]").attr("disabled", true);
    }
    var urllink = "../event/couponreward.asp?type=" + couponType;
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {
                    for (var ix = 1; ix <= 3; ix++) {
                        $("select[name=reward" + ix + "]").find('option').each(function () {
                            $(this).remove();
                        });
                        $("select[name=reward" + ix + "]").append("<option value=''>Reward</option>");
                        for (var i = 0; i < list.length; i++) {
                            $("select[name=reward" + ix + "]").append("<option value='" + list[i].ID + "'>" + list[i].desc + "</option>");
                        }
                    }
                }
            }
    });
}

function bossJoiner(raidID, openaid) {
    if ($("#joinerView").is(':visible')) {
        $("#joinerView").hide();
    }
    var urllink = "../loginfo/bossraidJoiner_view.asp?raidID=" + raidID + "&openaid=" + openaid;
    var info = ""
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {
                    info = "<table class='table table-bordered'><thead><tr><th>참여구분</th><th>닉네임</th><th>참여캐릭터</th><th>레벨</th><th>마지막 참여일시</th><th>누적 피해량</th></tr></thead>";
                    info = info + "<tbody>";
                    for (var i = 0; i < list.length; i++) {
                        info = info + "<tr><td>" + list[i].opentype + "</td><td>" + list[i].JoinerNick + "</td><td>" + list[i].classtype + "</td><td>" + list[i].JoinerLevel + "</td><td>" + list[i].UpdateDate + "</td><td>" + list[i].Damage + "</td></tr>";
                    }
                }
                info = info + "</tbody>";
                info = info + "</table>";
                $('#joiner').html(info);
            }
    });
    openLayer('joinerView', { top: 200 });
}

function guildpvpEntry(uniqueNumber, guildID, tablename, type) {
    if ($("#entryView" + type).is(':visible')) {
        $("#entryView" + type).hide();
    }
    var urllink = "../loginfo/guildpvp_view.asp?uniqueNumber=" + uniqueNumber + "&guildID=" + guildID + "&table=" + tablename;

    var info = ""
    $.ajax({
        url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                info = "<table class='table table-bordered'><thead><tr><th>닉네임</th><th>회원번호</th></tr></thead>";
                info = info + "<tbody>";
                if (list.length > 0) {
                    for (var i = 0; i < list.length; i++) {
                        info = info + "<tr><td>" + list[i].UserName + "</td><td>" + list[i].SNO + "</td></tr>";
                    }
                }
                info = info + "</tbody>";
                info = info + "</table>";
                $('#entry'+type).html(info);
            }
    });
    openLayer('entryView'+type, { top: 200 });
}

function bossreward(reward1, reward2, reward3, rewardDate, status) {
    if ($("#rewardView").is(':visible')) {
        $("#rewardView").hide();
    }
    if (rewardDate == "") {
        alert("획득 보상이 없습니다.");
    }
    else {
        $('#reward1').html(reward1);
        $('#reward2').html(reward2);
        $('#reward3').html(reward3);
        $('#rewardDate').html(rewardDate);
        openLayer('rewardView', { top: 200 });
    }
}

function guerrillaReward(reward1, reward2, rewardDate) {
    if ($("#rewardView").is(':visible')) {
        $("#rewardView").hide();
    }
    if (rewardDate == "" || (reward1 == "" && reward2 == 0)) {
        alert("획득 보상이 없습니다.");
    }
    else {
        $('#reward1').html(reward1);
        if (reward2 == 1000) {
            $('#reward2').html("공격력 증가 버프");
        }
        else if (reward2 == 1001) {
            $('#reward2').html("방어력 증가 버프");
        }
        $('#rewardDate').html(rewardDate);
        openLayer('rewardView', { top: 200 });
    }
}

function rewardTypeSelect(rewardType) {
    if (rewardType == 1) {
        $("#itemreward").hide();
        $("#soulreward").show();
        $("select[name=itemCode]").val("").attr("selected", "selected");
    }
    else if (rewardType == 2) {
        $("#soulreward").hide();
        $("#itemreward").show();
        $("select[name=class]").val("").attr("selected", "selected");
        $("select[name=grade]").val("").attr("selected", "selected");
        var urllink = "../event/couponrewardItem.asp";
        $.ajax({
            url: urllink
            , dataType: "json"
            , success: function (data) {
                var list = $(data);
                if (list.length > 0) {

                    $("select[name=itemCode]").find('option').each(function () {
                        $(this).remove();
                    });
                    $("select[name=itemCode]").append("<option value=''>Select</option>");
                    for (var i = 0; i < list.length; i++) {
                        $("select[name=itemCode]").append("<option value='" + list[i].item + "'>" + list[i].name + "</option>");
                    }

                }
            }
        });
    }
    else {
        $("#soulreward").hide();
        $("#itemreward").show();
        $("select[name=itemCode]").val("").attr("selected", "selected");
    }
}

function ChekNullIdx(formName, IdxName) {
    if (IdxName == "") {
        $("form").each(function () {
            if (this.id == formName) this.reset();
        });
    }
    else {
        if ($("input:hidden[name=" + IdxName + "]").length > 0) {
            $("input:hidden[name=" + IdxName + "]").val("");
            $("form").each(function () {
                if (this.id == formName) this.reset();
            });
        }
    }
    if ($('#deleteButton').length > 0){ //삭제 버튼 지우기
        $('#deleteButton').html("");
    }
}

function chageGuildState(aid, chageType,dbindex) {
    var chageString;
    if (chageType == 3) {
        chageString = "강제탈퇴";
    }
    else {
        chageString = "강제거절";
    }
    var check = confirm(chageString+" 하시겠습니까?");
    if (check == true) {
        $.ajax({
            type: "POST",
            url: "../gameinfo/guildwait_proc.asp",
            data: "aid=" + escape(aid) + "&changeType=" + escape(chageType) + "&db_index=" + escape(dbindex),
            success: function (msg) {
                if (msg == "OK") {
                    alert("ok.");
                    location.reload();
                } else {
                    alert("try again.");
                }
            }
        });
    }
}