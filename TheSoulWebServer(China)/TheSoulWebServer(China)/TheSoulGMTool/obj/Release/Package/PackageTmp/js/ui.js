
$(function(){
	$('.calendar').datepick({
		dateFormat: 'yy-mm-dd',
		showOn: 'button',
		buttonImage: '/img/btn_calendar.gif'
	})
	$(':checkbox').css({width:'13px',height:'13px'})
	$(':text,:file,textarea,:password').addClass('inputTxt');
	$('.table1 th:last').addClass('last'); //테이블1 th마지막 백그라운드제거
	$('.table2 tr').each(function(){$('td:last',this).addClass('last')})//테이블2 td마지막 백그라운드제거
	$('.list2').each(function(){$('li:last',this).css({backgroundImage:'none'})})
	$('.table2 tr:odd').addClass('odd');
	$('.paging .pages').children().each(function(){	//페이징 파이프라인설정	
		if($('.paging .pages').children().size() != 1 && $(this).index() + 1 != $('.paging .pages').children().size()){
			$(this).after(' |')	;
		}
	});

	//drawSelect();
	//$(":file").filestyle({ 
	//	 image: "/images/btn_fileSch.gif",
	//	 imageheight : 23,
	//	 imagewidth : 86,
	//	 width : 300
	// });	
	getData();
	imgtoggle();
	popClose();
	//fluidFlash('gnbFlash');
	//if(isie8){
	//	$('table').css({borderCollapse:'inherit'}).attr({cellspacing:'0',cellpadding:'0'});
		
	//}
});
$(window).bind('resize',function(){						 
	fluidFlash('gnbFlash');							 
});

function adjustFlash(id,w,h){	
	$('#'+id).attr({width:w,height:h});	
}

function faq(){
	var $faq = $('#faqList tbody');	
	
	$faq.children('tr:even').each(function(){
		$(this).find('td').addClass('question')										   
	});
	$faq.children('tr:odd').addClass('answer').hide();
	$faq.find('.question span').click(function(){		
		var $thisAnswer = $(this).closest('tr').next();
		$thisAnswer.siblings('tr.on').removeClass('on').hide()
		$faq.find('span.on').removeClass('on')
		if($thisAnswer.attr('class').indexOf('on') > -1){
			$thisAnswer.removeClass('on').hide()
			$(this).removeClass('on')
		} else{			
			$thisAnswer.addClass('on').show()
			$(this).addClass('on')
		}
	});	
}

function drawSelect(){
	var flashVersion = getFlashVersion();
	if(flashVersion=='0,0,0') return false;
	$('.oriUds').uds({
		newClass:'uds',
		limitLength : 10,
		rSpace: isSafari ? 35 : 15
	});
}



function adjustHeight(height){	
	$('#lnbMenu').attr('height',height)
}


//png24
function setPng24(obj) {
    obj.width=obj.height=1;
    obj.className=obj.className.replace(/\bpng24\b/i,'');
    obj.style.filter =
    "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+ obj.src+"',sizingMethod='image');"
    obj.src=''; 
    return '';
}

// popup
function popup(url,width,height){
	var filename = url.split(".asp")[0];
	var name = filename.split("/")[filename.split("/").length-1]; //name은 파일명으로 설정
	window.open(url,name,'width='+width+', height='+height+', scrollbars=no');
}

function fluidFlash(flashID){
	var minWidth = 980;	
	var $flash = $('#'+flashID)
	if($(window).width() <= minWidth){
		$flash.attr('width',minWidth);
		$flash.closest('div').css('width',minWidth+'px');
	} else {
		$flash.attr('width',$(window).width());
		$flash.closest('div').css('width',$(window).width()+'px');
	}
}

//Date구하기
function getData(){
	var obj_date = new Date();
	var to_day = obj_date.getDate();
	var to_month = obj_date.getMonth() +1 ;
	var to_year = obj_date.getFullYear();
	if (to_month < 10)
			to_month = "0" + to_month;
	if (to_day < 10)
			to_day = "0" + to_day;
	$('#year').html(to_year + " 년 ");
	$("#month").html(to_month + " 월 ");
	$("#day").html(to_day + " 일");
}

$.fn.JQmodal = function(option){ //event handler
	var settings = $.extend({			
		width     : 720,
		height    : 550,
		type      : 'layer',
		backColor : '#000',
		opacity   : 0.2,
		popUrl    : 'about:blank'
	},option);	

	return $(this).each(function(){
		var $this = $(this);			
		switch (settings.type){
			case 'layer' :
				$('<div id="modalBack" style="z-index:10001;position:absolute;left:0;top:0;width:'+document.documentElement.clientWidth+'px;height:'+$(document).height()+'px;background-color:'+settings.backColor+';opacity:'+settings.opacity+';filter:alpha(opacity='+(settings.opacity*100)+');"></div>').appendTo($('body'));
				$.each($.browser, function(name, val) {
					if(name == 'msie' && $.browser.version.substr(0,3) == '6.0'){
						$('<iframe src="about:blank" width="'+document.documentElement.clientWidth+'" height="'+$(document).height()+'" frameborder="0" scrolling="no" allowtransparency="true" style="position:absolute;left:0;top:0;height:100%;width:100%;filter:alpha(opacity=0);z-index:-1;"></iframe>').appendTo($('#modalBack'))						
						$("select",window.parent.document).css("display","none")
					}
				});
				$('<div id="modalWrap" style="z-index:10002;position:absolute;left:50%;top:'+(document.documentElement.scrollTop+document.documentElement.clientHeight/2)+'px;margin-left:'+(-settings.width/2)+'px;margin-top:'+(-settings.height/2)+'px;"><iframe src="'+settings.popUrl+'" width="'+settings.width+'" height="'+settings.height+'" allowtransparency="true" frameborder="0" scrolling="no"></div>').appendTo($('body'));
				
				$('#modalBack').click(function(){					
					$(this).next().find('iframe').attr('src','').end().remove().end().remove();
				})	;			
			break;
			case 'window' :
				windowModal = window.open(settings.popUrl,'','width='+settings.width+',height='+settings.height);
				windowModal.focus();
			break;			
		}
	});		
}
function popClose(){
	$('.popClose,.pop_cancel').click(function(){
		$("select",window.parent.document).css("display","inline");
		if(window.opener) {
			self.close();

		}
		else{
			$(".popWrap").css("display","none");
		}
		$('#modalWrap',window.parent.document).attr('src','');
		$('#modalBack',window.parent.document).remove();
		$('#modalWrap',window.parent.document).remove();
		
		
		return false;		
	});
}
function mainFootPop(url){
	alert(1)
	//$(document).JQmodal({popUrl:url});
}

//image toggle
function imgtoggle(){
	$(".imgtoggle").hover(function(){
		
		if ($(".imgtoggle").attr("src").indexOf("_on") > 0)
		{
			imgname = $(".imgtoggle").attr("src").split("_on");
			imgname = imgname[0].concat(imgname[1]);
			$(".imgtoggle").attr("src",imgname);
		}
		else
		{
			imgname = $(".imgtoggle").attr("src").split(".");
			imgname = imgname[0].concat("_on.").concat(imgname[1]);
			$(".imgtoggle").attr("src",imgname);
		}
	})
}