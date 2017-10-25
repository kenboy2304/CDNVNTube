$(document).ready(function() {
	//Top Menu
	topMenu();

	//Tabs 
	tabs();

	//Link Select
	inputShareLinkSelect();

	//Comment
	$('.fb_iframe_widget span').css({'width':'100%'});
	$('.fb_iframe_widget span iframe').css({'width':'100%'});

	//Pull
	$('.pull').click(function() {
		$('.main-nav-menu').slideToggle(500, "easeInOutQuad");
	});

	//Pull Search 
	$('.pull-search').click(function() {
		$('#search').slideToggle(250);
		$('#search-form .s-input').focus();
		$(this).hide();
		$('.close-pull-search').show();
	});

	//Pull Search 
	$('.close-pull-search').click(function() {
		$('#search').slideToggle(250);
		$(this).hide();
		$('.pull-search').show();
	});


	//Scalable Thumbnail 
	scaleThumb('.item .thumbnail');
	scaleThumb('.slides .thumbnail');
	
	// $('#slider-film .thumbnail').click(function() {
	// 	alert($('#slider-film .thumbnail').width());
	// 	return false;
	// });
	

});

function scaleThumb(id) {
	var tWidth = $(id).width();
	var tHeight = parseInt(tWidth * 9/16);
	$(id).height(tHeight);

	//When window is scrolling
	$(window).resize(function() {
		//Scalable Thumbnail 
		var tWidth = $(id).width();
		var tHeight = parseInt(tWidth * 9/16);
		$(id).height(tHeight);
	});
}

function tabs() {
	$( '#share' ).hide();
	var a = $( '#tabs a' );
	$( "a[href='#description']" ).addClass('current');

	var tab = $( '.tab' );
	a.click(function () {
		tab.hide();
		var id = $( this ).attr('href');
		a.removeClass('current');
		$( this ).addClass('current');
		$( id ).fadeIn();
		return false;
	});
}

function inputShareLinkSelect() {
	$( "a[href='#share']" ).click(function(){
		$( '#share input' ).select();
	});
} 

function topMenu() {
	var show_menu = $( '.show-menu' );
	var close_menu = $( '.close-menu' );
	var header = $( 'header' );

	show_menu.click(function() {
		header.animate({marginTop:"0px"}, 200);
		show_menu.hide();
		close_menu.show();
	});

	close_menu.click(function() {
		header.animate({marginTop:"-43px"}, 200);
		show_menu.show();
		close_menu.hide();
	});
}