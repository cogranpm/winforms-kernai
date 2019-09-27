namespace Kernai

module Main =

    open System
    open System.Drawing
    open System.Windows.Forms
    open KernaiGui.kernaigui


    [<EntryPoint>]
    let main argv =
        do Application.EnableVisualStyles()
        do Application.SetCompatibleTextRenderingDefault(false)
        let form = new MainForm(Text="Hello World Win Forms")
        do form.Init() |> ignore
        do form.Show()
        Application.Run(form)
        0 // return an integer exit code


