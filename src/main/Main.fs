module Main

open Fable.Core
open Fable.Core.JsInterop
open Electron
open Node.Api
open ELog

let mutable mainWindow: BrowserWindow option = Option.None


[<ImportAll("electron-log")>]
let [<Global>]  Log: ElectronLog.ElectronLog = jsNative


let appLogPath()=
  Electron.Electron.electron.app.getAppPath() + "/../../applog/"


let rendererLogPath()=
    appLogPath() + "renderer.log"

let mainLogPath()=
  appLogPath() + "main.log"



let setLog()=

    let logPath = mainLogPath()
    Browser.Dom.console.log("setLog appLogPath:",logPath)

    Log.transports.file.resolvePath <- (fun pv lm -> logPath)
    Log.transports.file.level <- !^ElectronLog.LogLevel.Silly

    Node.Api.``global``?Log <- Log
    Log.log("logPath:",logPath)


setLog()

let tstLog() =

  Log.log("simple log")
  Log.log("Data:",{|filed0="some data";filed2=2.2|})

  Log.error("simple error")
  Log.error("Data:",{|filed0="some data";filed2=2.1|})

tstLog()


let createMainWindow ( ) =

  let getRUrl()=
    #if DEBUG
    let port:string = ``process``.env?ELECTRON_WEBPACK_WDS_PORT
    let r_path  =   "renderer"
  
    //Browser.Dom.console.log("r_path:" + r_path )
    sprintf "http://localhost:%s/dist/%s/index.html" ``process``.env?ELECTRON_WEBPACK_WDS_PORT r_path
    #else
    sprintf "file:///%s" <| path.join(__dirname, "../admin/index.html")
    #endif
  

  let win =
    main.BrowserWindow.Create(jsOptions<BrowserWindowOptions>(fun o ->
      o.width <- 600
      o.height <- 800
      o.autoHideMenuBar <- true
      o.webPreferences <- jsOptions<WebPreferences>(fun w ->
        w.nodeIntegration <- true
        w.enableRemoteModule <- true
        w.webSecurity<- true
        w.nativeWindowOpen<- true
      )
      o.show <- false
    ))

  win.onceReadyToShow(fun _ ->
    win.show()
    
  ) |> ignore

  //#if DEBUG
  //win.webContents.onceDevtoolsFocused(fun _ ->
  //  JS.setTimeout win.webContents.focus 1 |> ignore
  //) |> ignore
  //win.webContents.openDevTools()
  //#endif

  // Load correct URL
  let url =  getRUrl()
  Log.log("Url:",url )
  url 
  |> win.loadURL
  |> ignore
    
  mainWindow <-  Some win
  


// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
main.app.onReady(fun _ _ -> createMainWindow() |> ignore   ) |> ignore


// Quit when all windows are closed.
main.app.onWindowAllClosed(fun _ ->
  // On OS X it's common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if ``process``.platform <> Node.Base.Platform.Darwin then
    main.app.quit()
) |> ignore


main.app.onActivate(fun _ _ ->
  // On OS X it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if mainWindow.IsNone  then createMainWindow() |> ignore
) |> ignore
