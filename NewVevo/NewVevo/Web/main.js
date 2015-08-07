//TODO, handle getting next video while playback is paused
// Output watch history to page

// TODO: styles
// constrain video to be 16:9
// don't show video container till streams load
// hover states on buttons


var token = "_TMw_fGgJHvzr84MqwK1eWhBgbdebZhAm_y3W1ou-sU1.1439085600.xrqkd87wbBX66Jh0rdWF_bDvOl6CfmhH_vc1-THLJjnmOfVeGM1dK14xiHsiZTSP7-jakA2";

// set up xhr and reusable request function
var ajax = function() {
  return new XMLHttpRequest();
}

var makeRequest = function(readyStateCallback, type, url, data) {
  var request = ajax();
  request.onreadystatechange = function() {
    if (request.readyState === 4) {
      readyStateCallback(request);
    }
  }

  request.open(type, url, true);
  if (type === "post") {
    request.setRequestHeader('Content-type', 'application/json');
  }
  request.send(data);
}

// utils
// parse seconds into timecode that server can ingest
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

var getMoreVideos = function(username, doNotGetNextVid) {
  cb = function(request) {
    if (request.readyState === 4) {
        randomVideos = randomVideos.concat(JSON.parse(request.response));

        //-- I know this looks weird
        if (!doNotGetNextVid)
            getNextVideo();
    }
  }

  makeRequest(cb, "get", "http://newvevo.azurewebsites.net/api/newvevo/Next?userId=" + username);
}

var getNextVideo = function(startPlay) {
  cb = function(request) {
    var response = JSON.parse(request.response);
    if (response.errors) {
      console.error("getNextVideo", response.errors);
      currentIndex++;
      return getNextVideo(startPlay);
    }
    video.src = response[2].url;
    video.load();
    if (startPlay) {
      video.play();
    }

    if (currentIndex < randomVideos.length - 5) {
        getMoreVideos(currentUser, true);
    }
  }
  isrc = randomVideos[currentIndex++].Isrc;
  makeRequest(cb, "get", "http://apiv2.vevo.com/video/" + isrc + "/streams/mp4?token=" + token);
}

var trackVideoWatch = function(roulette) {
  cb = function(request) {
    console.log(request);
    // if (response.error) {
    //   console.error("Mark Watched Failed", response.error);
    // }
  }
  data = JSON.stringify({
    userId: currentUser,
    isrc: randomVideos[currentIndex].Isrc,
    duration: getTimeCode(video.currentTime),
    IsRoulette: roulette,
    HasPressedPaused: false // TODO: hook this up
  })
  makeRequest(cb, "post", "http://newvevo.azurewebsites.net/api/newvevo/MarkWatched", data);
}

random.addEventListener('click', function (event) {
  trackVideoWatch();
  getNextVideo(true);
  addToWatchHistory();
});

username.addEventListener('keyup', function(event){
  if (event.keyCode === 13) {
    document.getElementsByClassName('username-container')[0].classList.toggle('active');
    document.getElementsByClassName('main-view')[0].classList.toggle('active');
    currentUser = username.value
    getMoreVideos(currentUser);
    showCoachMarks();
  }
});

playPauseBtn.addEventListener('click', function (event) {
  togglePlayPause();
});

rouletteBtn.addEventListener('click', function(event){
  // video.src = getNextVideo();
  playPauseBtn.classList.add('disable');
  randomBtn.classList.add('disable');
  trackVideoWatch(true);
  getNextVideo(true);
});

video.addEventListener('ended', function(event){
  trackVideoWatch();
  getNextVideo(true);
  playPauseBtn.classList.remove('disable');
  randomBtn.classList.remove('disable');
});

var hasPlayed = false;
video.addEventListener('play', function(){
  if(playPauseBtn.classList.contains('play')) {
    togglePlayPause(true);
  }
});


video.addEventListener('pause', function(){
  if(playPauseBtn.classList.contains('pause')) {
    togglePlayPause(true);
  }
});

var togglePlayPause = function (displayOnly) {
  classList = playPauseBtn.classList;
  if(classList.contains('play')) {
    classList.remove('play');
    classList.add('pause');
    if(!displayOnly) {
      video.play();
    }
  } else {
    classList.remove('pause');
    classList.add('play');
    if(!displayOnly) {
      video.pause();
    }
  }
}

var addToWatchHistory = function() {
  var videoIndex = currentIndex - 1;
  var div = document.createElement('div');
  var spanTitle = document.createElement('span');
  var spanArtist = document.createElement('span');
  var link = document.createElement('a');
  link.href = "http://www.vevo.com/watch/" + randomVideos[videoIndex].Isrc;
  spanTitle.innerHTML = "Title: " + randomVideos[videoIndex].Title;
  spanTitle.class = "title-item";
  spanArtist.innerHTML = "Artist: " + randomVideos[videoIndex].Artist;
  spanArtist.class = "artist-item";
  link.appendChild(spanTitle);
  link.appendChild(spanArtist);
  div.appendChild(link);
  var history = document.getElementById('history')
  history.appendChild(div);
}

var showCoachMarks = function() {
  var div = document.createElement('div');
  div.classList.add("coach-marks");
  div.innerHTML = "If you choose the Roulette button you will also get a random video, but you can\’t skip it\!"
  var buttonBar = document.getElementsByClassName('button-bar')[0];
  buttonBar.insertBefore(div, rouletteBtn);
}