module Entry

open Fable.Core
open Fable.Core.JsInterop

open ELog

[<ImportAll("electron-log")>]
let [<Global>]  Log: ElectronLog.ElectronLog = jsNative




let appLogPath()=

  Electron.Electron.electron.remote.app.getAppPath() + "/../../applog/"


let rendererLogPath()=
    appLogPath() + "renderer.log"

let mainLogPath()=
  appLogPath() + "main.log"



let setLog()=

   let p =  rendererLogPath()
   Log.transports.file.resolvePath <- (fun pv lm -> p)
   Log.transports.file.level <- !^ElectronLog.LogLevel.Silly
   Browser.Dom.window?Log <- Log



setLog()

let tstLog() =

  Log.log("simple log")
  Log.log("Data:",{|filed0="some data";filed2=2.2|})

  Log.error("simple error")
  Log.error("Data:",{|filed0="some data";filed2=2.1|})


  Browser.Dom.document.getElementById("msg0").textContent<- rendererLogPath()
  Browser.Dom.document.getElementById("msg1").textContent<- mainLogPath()

tstLog()






