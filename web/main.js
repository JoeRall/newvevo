var token = "_TMw_fGgJHvzr84MqwK1eWhBgbdebZhAm_y3W1ou-sU1.1439085600.xrqkd87wbBX66Jh0rdWF_bDvOl6CfmhH_vc1-THLJjnmOfVeGM1dK14xiHsiZTSP7-jakA2";

// set up initial xhr
var ajax = function() {
  return new XMLHttpRequest();
}


// get dom elements
var video = document.getElementById('video');
var rouletteBtn = document.getElementById('roulette');
var playPauseBtn = document.getElementById('playpause');
var randomBtn = document.getElementById('random');
var username = document.getElementById('username');

// init video array and index
var randomVideos = [];
var currentIndex = 0;

// store the user
var currentUser = "";

var getMoreVideos = function(username) {
  var request = ajax();
  request.onreadystatechange = function() {
    if (request.readyState === 4) {
      console.log("response", JSON.parse(request.response));
      randomVideos = randomVideos.concat(JSON.parse(request.response));
      getNextVideo();
    }
  }

  request.open("get", "http://newvevo.azurewebsites.net/api/newvevo/Next?userId=" + username, true);
  request.send(null);
}

var getNextVideo = function(startPlay) {
  var request = ajax();
  request.onreadystatechange = function() {
    if (request.readyState === 4) {
      console.log(request.response);
      video.src = (JSON.parse(request.response))[0].url;
      video.load();
      if (startPlay) {
        video.play();
      }
    }
  }
  isrc = randomVideos[currentIndex++].Isrc;
  request.open("get", "http://apiv2.vevo.com/video/" + isrc + "/streams/mp4?token=" + token, true);
  request.send(null);
}

var getTimeCode = function(seconds) {
  var sec_num = parseInt(seconds, 10); // don't forget the second param
  var hours   = Math.floor(sec_num / 3600);
  var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
  var seconds = sec_num - (hours * 3600) - (minutes * 60);

  if (hours   < 10) {hours   = "0"+hours;}
  if (minutes < 10) {minutes = "0"+minutes;}
  if (seconds < 10) {seconds = "0"+seconds;}
  var time    = hours+':'+minutes+':'+seconds;
  return time;
}

random.addEventListener('click', function (event) {
  // some random object of videos, get next
  // play the next video
  // check if we need more vidoes
  // if yes make a call to the server
  // randomVideos.concat(getMoreVideos());

  var request = ajax();
  request.onreadystatechange = function() {
    if (request.readyState === 4) {
    }
  }
  request.open("post", "http://newvevo.azurewebsites.net/api/newvevo/MarkWatched", true);
  request.setRequestHeader('Content-type', 'application/json');
  request.send(JSON.stringify({
    userId: currentUser,
    isrc: randomVideos[currentIndex].Isrc,
    duration: getTimeCode(video.currentTime)
  }));
  getNextVideo(true);
});

username.addEventListener('keyup', function(event){
  if (event.keyCode === 13) {
    document.getElementsByClassName('username-container')[0].classList.toggle('active');
    document.getElementsByClassName('video-container')[0].classList.toggle('active');
    currentUser = username.value
    getMoreVideos(currentUser);
  }
});

playPauseBtn.addEventListener('click', function (event) {
  classList = playPauseBtn.classList;
  if(classList.contains('play')) {
    classList.remove('play');
    classList.add('pause');
    video.play();
  } else {
    classList.remove('pause');
    classList.add('play');
    video.pause();
  }
});

rouletteBtn.addEventListener('click', function(event){
  // video.src = getNextVideo();
  playPauseBtn.classList.add('disable');
  randomBtn.classList.add('disable');
});

video.addEventListener('ended', function(event){
  playPauseBtn.classList.remove('disable');
  randomBtn.classList.remove('disable');
});

var addToWatchHistory = function() {

}
