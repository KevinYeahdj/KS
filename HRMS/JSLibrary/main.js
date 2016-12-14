// JavaScript Document
$(function(){
	// 个性化设置
	$(".pull-right .setToggle").click(function(){
	$(".setbox").slideToggle(300);
	});
	
	var navfixed = $(window).height() - 50
	$(".sidebar-fixed").height(navfixed);
	
	var navTop = $(".navbar");
		leftbar = $(".page-sidebar");
		content = $(".page-content");	
		selectbox = $(".selectbox select");
		mainContent = $(".main-container");
		footer = $('.footer');
		pw = leftbar.width();	
	
	selectbox.change(function(){	
		var name = $(this).find("option:selected").attr("name");		
		switch(name){ 
				case "absolute":  //条件判断
					navTop.removeClass("navbar-fixed");
					leftbar.removeClass("sidebar-fixed");
					footer.removeClass("footer-fixed");
					break;
					
				case "fixed":    
                	navTop.addClass("navbar-fixed");
					leftbar.addClass("sidebar-fixed");
					footer.addClass("footer-fixed");
					break;
					
				case "left":
					leftbar.removeClass("nav-right");
					content.css({"margin-left":"220px","margin-right":"0px"});
					break;
					
				case "right":
					leftbar.addClass("nav-right");
					content.css({"margin-left":"0px","margin-right":"220px"});
					break;
					
				case "border":	
					mainContent.removeClass("shadow");
					break;
					
				case "shadow":	
					mainContent.addClass("shadow");
					break;	
					
				case "nobr":	
					content.addClass("not-br");
					break;
					
				case "br":	
					content.removeClass("not-br");
					break;		
						
				case "small":	
					$("body").css({"font-size":"12px"});
					break;
					
				case "middle":	
					$("body").css({"font-size":"14px"});
					break;
					
				case "big":	
					$("body").css({"font-size":"15px"});
					break;			
		}
	});  
	
	$('.icheckbox').on("click",function(){
		$(this).toggleClass('checked');
	});    	 

	//左侧导航
	var $activeLi = $('.sidebar-menu > li.open');
	var $menuLi = $('.sidebar-menu > li:not(".line")');
	$menuLi.last().addClass('last');
	var headerHeight = $('.header').outerHeight(),
	    footerHeight = $('.footer').outerHeight(),
        MenuMH = $(window).height() - headerHeight - footerHeight;
		
	$('.page-sidebar').css({"height":$('.main-container').height()},{"min-height":MenuMH});
	
	// 添加图标
	var $sidebarUl = $('.sidebar-menu');
	$sidebarUl.find('a').each(function(index, element) {
        if( $(this).next().hasClass('sub-menu') ){
			$(this).append('<span class="icon-arrow-down2"></span>');
		}
    });

	$('ul.sidebar-menu  li  a').click(function(e){
		if( $(this).next().is(':hidden') ){
				$(this).find('span').removeClass('icon-arrow-down2').addClass('icon-arrow-up2');
		}else{
			$(this).find('span').removeClass('icon-arrow-up2').addClass('icon-arrow-down2');
		}
		var $_thisLI = $(this).parent('li');
		var $subUl = $(this).next();
		//var $subUl = $_thisLI.find('.sub-menu');
		$_thisLI.addClass('active open').siblings().removeClass('active open');		
		$_thisLI.siblings().find('.sub-menu').slideUp(200);
		$_thisLI.siblings().find('span').removeClass('icon-arrow-up2').addClass('icon-arrow-down2');
		if( $_thisLI.is(':has(ul)') ) {
		//	$(this).find('span').toggleClass('icon-arrow-up2');
			$subUl.slideToggle(200);
		}
	});
	
	
	$('#skin li').click(function(){
		$("#"+this.id).addClass("selected").siblings().removeClass("selected");
		var skinCssPath = "skin/"+(this.id)+".css";
		$('#skinCss').attr("href",skinCssPath);
		//$.cookie(skinCookieName,skinCssPath,{expires:30,path:'/'});
	});
	 
	
});

