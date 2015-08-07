//
//  ViewController.swift
//  VideoRoulette
//
//  Created by Daniel Gallardo on 8/6/15.
//  Copyright (c) 2015 Daniel Gallardo. All rights reserved.
//

import UIKit

import MediaPlayer

import Alamofire

class ViewController: UIViewController {

    var moviePlayer: MPMoviePlayerController!
    
    @IBOutlet weak var randomButton: UIButton!
    
    @IBOutlet weak var rouletteButton: UIButton!
    
    @IBOutlet weak var playButton: UIButton!
    
    var requestedIsrc = String()
    
    var counter = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        //check if user is first time
        
        self.navigationController?.navigationBarHidden = true

        randomButton.layer.cornerRadius = randomButton.frame.width/2
        rouletteButton.layer.cornerRadius = rouletteButton.frame.width/2
        
        println(User.sharedInstance.email)

        self.requestVideo(counter)
        
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
    func displayVideoFromURL( address: String ) {
        
        println(address)
        
        if let url:NSURL = NSURL( string: address ) {
            
            moviePlayer = MPMoviePlayerController()
            
            if  (moviePlayer != nil) {
                
                moviePlayer.prepareToPlay()
                moviePlayer.view.frame = CGRectMake(0, 0, view.frame.size.width, 240)
                
                self.view.addSubview(moviePlayer.view)
                moviePlayer.controlStyle = MPMovieControlStyle.Embedded
                moviePlayer.movieSourceType = MPMovieSourceType.Streaming
                moviePlayer.controlStyle = MPMovieControlStyle.None
                
                moviePlayer.contentURL = url
                
                moviePlayer.play()
            }
        }
    }
    
   //Button Methods
    @IBAction func rouletteButtonPressed(sender: AnyObject) {
        
        //Play next video 
        
        //send request to let server know where video was skipped

        
        let currentTime = moviePlayer.currentPlaybackTime
        
        UIView.animateWithDuration(0.35, delay: 0, options: .CurveEaseOut, animations: {
            self.randomButton.alpha = 0
            }, completion: { finished in
        })
    }
    
    
    @IBAction func randomButtonPressed(sender: AnyObject) {
        //Play next video
        

        
        if(counter != 10){
            if(counter == 8){
                //call more videos
                self.geMoreVideos()
                counter = 0
            }else{
                counter++
                self.requestVideo(counter)
            }
            
        }
        
    }
    
    
    @IBAction func playButtonPressed(sender: AnyObject) {
            
            switch moviePlayer.playbackState{
            case MPMoviePlaybackState.Playing:
                playButton.setTitle("Pause", forState: UIControlState.Normal)
                playButton.setImage(UIImage(named: "play"), forState: UIControlState.Normal)
                moviePlayer.pause()
            case MPMoviePlaybackState.Paused:
                playButton.setImage(UIImage(named: "pause"), forState: UIControlState.Normal)
                moviePlayer.play()
            default:
                println("do nothing")
            }
    }
    
    
    override func prefersStatusBarHidden() -> Bool {
        return true
    }
    
    func requestVideo(integer: Int){
        
        requestedIsrc = User.sharedInstance.videosArray[counter]["Isrc"] as! String
        Alamofire.request(.GET, "http://apiv2.vevo.com/video/\(requestedIsrc)/streams/mp4?token=_TMw_fGgJHvzr84MqwK1eWhBgbdebZhAm_y3W1ou-sU1.1439085600.xrqkd87wbBX66Jh0rdWF_bDvOl6CfmhH_vc1-THLJjnmOfVeGM1dK14xiHsiZTSP7-jakA2", parameters: nil)
            .responseJSON { request, response, data, error in
                println(request)
                println(response)
                println(error)
                
                let urlString = data?[0]["url"]
                
                self.displayVideoFromURL(urlString as! String)
        }
    }
    
    func geMoreVideos(){
        Alamofire.request(.GET, "http://newvevo.azurewebsites.net/api/newvevo/Next?userId=\(User.sharedInstance.email)", parameters: nil)
            .responseJSON { request, response, data, error in
                println(request)
                println(response)
                
                //crash
                User.sharedInstance.videosArray.addObjectsFromArray(data! as! [AnyObject])
                self.requestVideo(self.counter)
        }
    }
    

}

