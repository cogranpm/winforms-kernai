namespace KernaiGui

open System
open System.IO
open System.Text
open System.Drawing
open System.Windows.Forms
open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.Interactive.Shell

open database_functions


module kernaigui =

    let txtInstructions =  new TextBox(Multiline = true, Anchor = (AnchorStyles.Top ||| AnchorStyles.Right ||| AnchorStyles.Left))
    let txtInteractive = new TextBox(Multiline = true, Anchor = (AnchorStyles.Top ||| AnchorStyles.Right ||| AnchorStyles.Left ||| AnchorStyles.Bottom))
     

    let sbOut = new StringBuilder()
    let sbErr = new StringBuilder()
    let inStream = new StringReader("")
    let outStream = new StringWriter(sbOut)
    let errStream = new StringWriter(sbErr)

    let argv = [| "\usr\lib\mono\fsharp\fsi.exe" |]
    let allArgs = Array.append argv [|"--noninteractive"|]

    let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
    let fsiSession = FsiEvaluationSession.Create(fsiConfig, allArgs, inStream, outStream, errStream)  

    //returns value from script as an obj
    //right now this is being converted as a string
    let evalExpression text =
      match fsiSession.EvalExpression(text) with
      | Some value -> sprintf "%A" value.ReflectionValue
      | None -> "Got no result!"

    //note that ;; is not required for terminating lines in script
    let evalInteraction text =
        let result, warnings = fsiSession.EvalInteractionNonThrowing text
        match result with 
        | Choice1Of2() -> printfn "script executed ok"
        | Choice2Of2 exn -> printfn "execution error: %s " exn.Message
        for w in warnings do 
           printfn "Warning %s at %d,%d" w.Message w.StartLineAlternate w.StartColumn 
        txtInteractive.Text <- txtInteractive.Text + "\n" + outStream.ToString()


    //sample for evaluating a script
    //File.WriteAllText("sample.fsx", "let twenty = 10 + 10")
    //fsiSession.EvalScript "sample.fsx"

    let getFPText = 
        //evalExpression "2 + 1"
        "Type a script in there below and click execute:"



    type MainForm() =
        inherit Form()
        member f.Instructions with get() = txtInstructions
        member f.Interactive with get() = txtInteractive

        member f.Init() =

            let layout = new TableLayoutPanel(ColumnCount = 2, RowCount = 1)
            let buttonsLayout = new FlowLayoutPanel(FlowDirection = FlowDirection.TopDown)
            let textSpace = new TableLayoutPanel(ColumnCount = 1, RowCount = 3)
            textSpace.Dock <- DockStyle.Fill

            layout.Dock <- DockStyle.Fill
            let buttonFP = new Button(Text="Functional Programming", AutoSizeMode=AutoSizeMode.GrowAndShrink, Anchor = (AnchorStyles.Top ||| AnchorStyles.Left ))
            buttonFP.AutoSize <- true
            buttonFP.Click.Add(fun _ -> f.Instructions.Text <- getFPText)

            let buttonExecute = new Button(Text="Execute")
            buttonExecute.Click.Add(fun _ -> evalInteraction txtInteractive.Text)
            //f.Instructions.Dock <- DockStyle.Fill
            //f.Interactive.Dock <- DockStyle.Fill

            //layout.Controls.Add(buttonsLayout)
            //layout.Controls.Add(textSpace)

            buttonsLayout.Controls.Add(buttonFP)     
            textSpace.Controls.Add(f.Instructions)
            textSpace.Controls.Add(buttonExecute)
            textSpace.Controls.Add(f.Interactive)

            let splitContainer = new SplitContainer()
            splitContainer.Dock <- DockStyle.Fill
            splitContainer.Orientation <- Orientation.Vertical
            splitContainer.SplitterWidth <- 10
            splitContainer.Panel1.Controls.Add(buttonsLayout)
            splitContainer.Panel2.Controls.Add(textSpace)


            //window trim
            let menu = new MenuStrip()
            let fileMenuItem = new ToolStripMenuItem("&File")
            let exitMenuItem = new ToolStripMenuItem("&Exit")
            //exitMenuItem.ShortcutKeys <- CtrlF
            fileMenuItem.DropDownItems.Add(exitMenuItem) |> ignore
            menu.Items.Add(fileMenuItem) |> ignore
            exitMenuItem.Click.Add(fun _ -> f.Close())
            
            f.MainMenuStrip <- menu

            let statusProgress =
                new ToolStripProgressBar(Size = new Size(200, 16),
                    Style = ProgressBarStyle.Marquee,
                    Visible = false)
            let status = new StatusStrip(Dock=DockStyle.Bottom)
            status.Items.Add(statusProgress) |> ignore

            let toolbar = new ToolStrip(Dock=DockStyle.Top)
            let toolBtn = new ToolStripButton(DisplayStyle=ToolStripItemDisplayStyle.Text, Text="New")
            toolBtn.Click.Add(fun arg -> printfn "hello world")
            toolbar.Items.Add(toolBtn) |> ignore

            f.Controls.Add(f.MainMenuStrip)
            f.Controls.Add(toolbar)
            f.Controls.Add(splitContainer)
            //f.Controls.Add(layout)
            f.Controls.Add(status)
            toolbar.BringToFront()
            f.PerformLayout()

            //textSpace.Controls.Add(f.Interactive)
            0
            //this is testing the database stuff
            //testing



    type kernaigui() =
        let name = "fred"