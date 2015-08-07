//
//  LoginViewController.swift
//  VideoRoulette
//
//  Created by Daniel Gallardo on 8/6/15.
//  Copyright (c) 2015 Daniel Gallardo. All rights reserved.
//

import UIKit

import Alamofire

import SwiftyJSON

class LoginViewController: UIViewController, UITextFieldDelegate, UIAlertViewDelegate {
    
    
    @IBOutlet weak var emailTextField: UITextField!
    
    var counter = 0
    
    
    override func viewDidLoad() {
        
        emailTextField.delegate = self
        
        self.navigationController?.navigationBarHidden = true
        
        emailTextField.becomeFirstResponder()
        
        
        
    }
    
    func textFieldShouldReturn(textField: UITextField) -> Bool {
        
        
        var storyboard = UIStoryboard(name: "Main", bundle: nil)
        var movieViewController = storyboard.instantiateViewControllerWithIdentifier("movieViewController") as! UIViewController
        
        
        if (emailTextField.text != ""){
            User.sharedInstance.email = textField.text
            
            Alamofire.request(.GET, "http://newvevo.azurewebsites.net/api/newvevo/Next?userId=\(User.sharedInstance.email)", parameters: nil)
                .responseJSON { request, response, data, error in
                    println(request)
                    println(response)
                    
                    //crash
                    User.sharedInstance.videosArray.addObjectsFromArray(data! as! [AnyObject])
                    
                    if (error != nil){
                        self.showAlert()
                    }else{
                        self.navigationController?.pushViewController(movieViewController, animated: true)
                        self.emailTextField.resignFirstResponder()
                    }
            }
            
            
            
        }else{
            
            self.showAlert()
            return false
        }
        return true
    }
    
    func showAlert(){
        
        var alert = UIAlertController(title: "Username", message: "Please enter a valid username", preferredStyle: UIAlertControllerStyle.Alert)
        alert.addAction(UIAlertAction(title: "OK", style: UIAlertActionStyle.Default, handler: nil))
        self.presentViewController(alert, animated: true, completion: nil)
    }
    
    override func prefersStatusBarHidden() -> Bool {
        return true
    }
}
