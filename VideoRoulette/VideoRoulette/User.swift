//
//  User.swift
//  VideoRoulette
//
//  Created by Daniel Gallardo on 8/6/15.
//  Copyright (c) 2015 Daniel Gallardo. All rights reserved.
//

import UIKit

class User {
    
    var email: String = ""
    var videosArray = NSMutableArray()
    var isrc = ""
    
    class var sharedInstance: User {
        struct Static {
            static let instance : User = User()
        }
        return Static.instance
    }
    
}
