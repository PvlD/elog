// ts2fable 0.7.1
module rec ELog
open System
open System.ComponentModel
open Fable.Core
open Fable.Core.JS


type Array<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception



type RequestOptions  = Node.Http.RequestOptions

type [<AllowNullLiteral>] InspectOptions =
       /// If set to `true`, getters are going to be
       /// inspected as well. If set to `'get'` only getters without setter are going
       /// to be inspected. If set to `'set'` only getters having a corresponding
       /// setter are going to be inspected. This might cause side effects depending on
       /// the getter function.
       abstract getters: U2<bool, string> option with get, set
       abstract showHidden: bool option with get, set
       abstract depth: float option with get, set
       abstract colors: bool option with get, set
       abstract customInspect: bool option with get, set
       abstract showProxy: bool option with get, set
       abstract maxArrayLength: float option with get, set
       /// Specifies the maximum number of characters to
       /// include when formatting. Set to `null` or `Infinity` to show all elements.
       /// Set to `0` or negative to show no characters.
       abstract maxStringLength: float option with get, set
       abstract breakLength: float option with get, set
       /// Setting this to `false` causes each object key
       /// to be displayed on a new line. It will also add new lines to text that is
       /// longer than `breakLength`. If set to a number, the most `n` inner elements
       /// are united on a single line as long as all properties fit into
       /// `breakLength`. Short array elements are also grouped together. Note that no
       /// text will be reduced below 16 characters, no matter the `breakLength` size.
       /// For more information, see the example below.
       abstract compact: U2<bool, float> option with get, set
       abstract sorted: U2<bool, (string -> string -> float)> option with get, set





module ElectronLog =
 
    type [<StringEnum>] [<RequireQualifiedAccess>] LogLevel =
        | Error
        | Warn
        | Info
        | Verbose
        | Debug
        | Silly

    type LevelOption = U2<LogLevel, bool>
        
        

    type [<AllowNullLiteral>] Levels =
        interface end

    type [<AllowNullLiteral>] Format =
        [<Emit "$0($1...)">] abstract Invoke: message: LogMessage * ?transformedData: ResizeArray<obj option> -> U2<ResizeArray<obj option>, string>

    type [<StringEnum>] [<RequireQualifiedAccess>] FopenFlags =
    
        | [<CompiledName "r+">] ``R``
        | [<CompiledName "rs+">] ``Rs``
        | [<CompiledName "w+">] ``W``
        | [<CompiledName "wx+">] ``Wx``
        | [<CompiledName "a+">] ``A``
        | [<CompiledName "ax+">] ``Ax``


    type [<AllowNullLiteral>] Hook =
        [<Emit "$0($1...)">] abstract Invoke: message: LogMessage * ?selectedTransport: Transport -> LogMessage

    type [<AllowNullLiteral>] Variables =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: name: string -> obj option with get, set

    type [<AllowNullLiteral>] LogMessage =
        /// Any arguments passed to a log function
        abstract data: ResizeArray<obj option> with get, set
        /// When the log entry was created
        abstract date: DateTime with get, set
        /// From error to silly
        abstract level: LogLevel with get, set
        /// Message scope label
        abstract scope: string option with get, set
        /// Variables used by formatter
        abstract variables: Variables option with get, set

    type [<AllowNullLiteral>] Transport =
        [<Emit "$0($1...)">] abstract Invoke: message: LogMessage -> unit
        /// Messages with level lower than will be dropped
        abstract level: LevelOption with get, set

    type [<AllowNullLiteral>] ConsoleTransport =
        inherit Transport
        /// String template of function for message serialization
        abstract format: U2<Format, string> with get, set
        /// Use styles even if TTY isn't attached
        abstract useStyles: bool with get, set

    type [<AllowNullLiteral>] PathVariables =
        /// Per-user application data directory, which by default points to:
        /// %APPDATA% on Windows
        /// $XDG_CONFIG_HOME or ~/.config on Linux
        /// ~/Library/Application Support on macOS
        abstract appData: string
        /// Application name from productName or name of package.json
        abstract appName: string
        /// Application version from package.json
        abstract appVersion: string
        /// app.getPath('logs'). May be unavailable in old versions
        abstract electronDefaultDir: string option
        /// Name of the log file without path
        abstract fileName: string option
        /// User's home directory
        abstract home: string
        /// userData + /logs/ + fileName on Linux and Windows
        /// ~/Library/Logs/ + appName + / + fileName on macOS
        abstract libraryDefaultDir: string
        /// Same as libraryDefaultDir, but contains '{appName}' template instead
        /// of the real application name
        abstract libraryTemplate: string
        /// OS temporary path
        abstract tempDir: string
        /// The directory for storing your app's configuration files, which by
        /// default it is the appData directory appended with your app's name.
        abstract userData: string

    type [<AllowNullLiteral>] WriteOptions =
        /// Default 'a'
        abstract flag: FopenFlags option with get, set
        /// Default 0666
        abstract mode: float option with get, set
        /// Default 'utf8'
        abstract encoding: string option with get, set

    type [<AllowNullLiteral>] LogFile =
        /// Full log file path
        abstract path: string
        /// How many bytes were written since transport initialization
        abstract bytesWritten: float
        /// Current file size
        abstract size: float
        /// Clear the log file
        abstract clear: unit -> bool
        /// Emitted when there was some error while saving log file
        [<Emit "$0.on('error',$1)">] abstract on_error: listener: (Error -> LogFile -> unit) -> LogFile

    type [<AllowNullLiteral>] FileTransport =
        inherit Transport
        /// Determines a location of log file, something like
        /// ~/.config/<app name>/log.log depending on OS. By default electron-log
        /// reads this value from name or productName value in package.json. In most
        /// cases you should keep a default value
        abstract appName: string option with get, set
        /// Function which is called on log rotation. You can override it if you need
        /// custom log rotation behavior. This function should remove old file
        /// synchronously
        abstract archiveLog: (string -> unit) with get, set
        /// How many bytes were written since transport initialization
        abstract bytesWritten: float with get, set
        /// How deep to serialize complex objects
        /// Deprecated in favor of inspectOptions
        abstract depth: float with get, set
        /// The full log file path. I can recommend to change this value only if
        /// you strongly understand what are you doing. If set, appName and fileName
        /// options are ignored
        abstract file: string option with get, set
        /// Filename without path, main.log (or renderer.log) by default
        abstract fileName: string with get, set
        /// String template of function for message serialization
        abstract format: U2<Format, string> with get, set
        /// Return the current log file instance
        /// You only need to provide message argument if you define log path inside
        /// resolvePath callback depending on a message.
        abstract getFile: ?message: obj -> LogFile
        /// Serialization options
        abstract inspectOptions: InspectOptions with get, set
        /// Maximum size of log file in bytes, 1048576 (1mb) by default. When a log
        /// file exceeds this limit, it will be moved to log.old.log file and the
        /// current file will be cleared. You can set it to 0 to disable rotation
        abstract maxSize: float with get, set
        /// Reads content of all log files
        abstract readAllLogs: unit -> Array<FileTransportReadAllLogsArray>
        /// Allow to change log file path dynamically
        abstract resolvePath: (PathVariables -> LogMessage -> string) with get, set
        /// Whether to write a log file synchronously. Default to true
        abstract sync: bool with get, set
        /// Options used when writing a file
        abstract writeOptions: WriteOptions option with get, set
        /// Clear the current log file
        abstract clear: unit -> unit
        /// Return full path of the current log file
        abstract findLogPath: ?appName: string * ?fileName: string -> string
        /// In most cases, you don't need to call it manually. Try to call only if
        /// you change appName, file or fileName property, but it has no effect.
        abstract init: unit -> unit

    type [<AllowNullLiteral>] RemoteTransport =
        inherit Transport
        /// Client information which will be sent in each request together with
        /// a message body
        abstract client: obj option with get, set
        /// How deep to serialize complex objects
        abstract depth: float option with get, set
        /// Additional options for the HTTP request
        abstract requestOptions: RequestOptions option with get, set
        /// Callback which transforms request body to string
        abstract transformBody: (obj -> string) option with get, set
        /// Server URL
        abstract url: string with get, set

    type [<AllowNullLiteral>] Transports =
        /// Writes logs to console
        abstract console: ConsoleTransport with get, set
        /// Writes logs to a file
        abstract file: FileTransport with get, set
        /// When logging inside renderer process, it shows log in application
        /// console too and vice versa. This transport can impact on performance,
        /// so it's disabled by default for packaged application.
        abstract ipc: Transport option with get, set
        /// Sends a JSON POST request with LogMessage in the body to the specified url
        abstract remote: RemoteTransport with get, set
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Transport option with get, set

    type [<AllowNullLiteral>] Scope =
        [<Emit "$0($1...)">] abstract Invoke: label: string -> LogFunctions
        /// Label for log message without scope. False value disables padding
        /// when labelPadding is enabled.
        abstract defaultLabel: string with get, set
        /// Pad scope label using spaces
        /// false: disabled
        /// true: automatically
        /// number: set exact maximum label length. Helpful when a scope can
        /// be created after some log messages were sent
        abstract labelPadding: U2<bool, float> with get, set

    type [<AllowNullLiteral>] ReportData =
        abstract body: string with get, set
        abstract title: string with get, set
        abstract assignee: string with get, set
        abstract labels: string with get, set
        abstract milestone: string with get, set
        abstract projects: string with get, set
        abstract template: string with get, set

    type [<AllowNullLiteral>] CatchErrorsOptions =
        /// Default true for the main process. Set it to false to prevent showing a
        /// default electron error dialog
        abstract showDialog: bool option with get, set
        /// Attach a custom error handler. If the handler returns false, this error
        /// will not be processed
        abstract onError: error: Error * ?versions: CatchErrorsOptionsOnErrorVersions * ?submitIssue: (string -> U2<ReportData, obj option> -> unit) -> unit

    type [<AllowNullLiteral>] CatchErrorsOptionsOnErrorVersions =
        abstract app: string with get, set
        abstract electron: string with get, set
        abstract os: string with get, set

    type [<AllowNullLiteral>] CatchErrorsResult =
        /// Stop catching errors
        abstract stop: unit -> unit

    
    
    [<EditorBrowsable(EditorBrowsableState.Never)>]
     type   [<AllowNullLiteral>]  LogFunctions =
        /// Log an error message
        //abstract error: [<ParamArray>] ``params``: ResizeArray<obj > -> unit
        abstract error: [<ParamArray>] ``params``: obj[] -> unit
        /// Log a warning message
        //abstract warn: [<ParamArray>] ``params``: ResizeArray<obj > -> unit
        abstract warn: [<ParamArray>] ``params``: obj[]  -> unit
        /// Log an informational message
        //abstract info: [<ParamArray>] ``params``: ResizeArray<obj > -> unit
        abstract info: [<ParamArray>] ``params``: obj[] -> unit
        /// Log a verbose message
        //abstract verbose: [<ParamArray>] ``params``: ResizeArray<obj > -> unit
        abstract verbose: [<ParamArray>] ``params``: obj[] -> unit
        /// Log a debug message
        //abstract debug: [<ParamArray>] ``params``: ResizeArray<obj > -> unit
        abstract debug: [<ParamArray>] ``params``: obj[]  -> unit
        /// Log a silly message
        //abstract silly: [<ParamArray>] ``params``: ResizeArray<obj > -> unit
        abstract silly: [<ParamArray>] ``params``: obj[]  -> unit
        /// Shortcut to info
        //abstract log: [<ParamArray>] ``params``: ResizeArray<obj> -> unit

        abstract log: [<ParamArray>] ``params``: obj[] -> unit
        


   
    type [<AllowNullLiteral>] ElectronLog =
        inherit LogFunctions
        /// Object contained only log functions
        abstract functions: LogFunctions with get, set
        /// Transport instances
        abstract transports: Transports with get, set
        /// Array with all attached hooks
        abstract hooks: ResizeArray<Hook> with get, set
        /// Array with all available levels
        abstract levels: Levels with get, set
        /// Variables used by formatters
        abstract variables: Variables with get, set
        /// Catch and log unhandled errors/rejected promises
        abstract catchErrors: ?options: CatchErrorsOptions -> CatchErrorsResult
        /// Create a new electron-log instance
        abstract create: logId: string -> ElectronLog.ElectronLog
        /// Create a new scope
        abstract scope: Scope with get, set
        /// Low level method which logs the message using specified transports
        abstract logMessageWithTransports: message: LogMessage * transports: ResizeArray<Transport> -> unit

    type [<AllowNullLiteral>] FileTransportReadAllLogsArray =
        abstract path: string with get, set
        abstract lines: ResizeArray<string> with get, set


let [<Global>]  Log: ElectronLog.ElectronLog = null



