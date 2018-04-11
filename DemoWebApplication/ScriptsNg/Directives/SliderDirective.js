app.directive('skdslider', function () {
  
  return {
    link: function (scope, element, attrs) {
      
        element.skdslider({
            delay: 5000, 
            animationSpeed: 2000,
            showNextPrev: true,
            showPlayButton: true,
            autoSlide: true,
            animationType: 'fading'
      });
    }
  }
});