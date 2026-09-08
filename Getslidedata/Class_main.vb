'Imports Autodesk.AutoCAD
'Imports System.Runtime.InteropServices
'Imports Autodesk

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.CompilerServices
'Imports System.Windows.Forms

' 核心运行时和应用程序服务
Imports ZwSoft.ZwCAD.Runtime
Imports ZwSoft.ZwCAD.ApplicationServices

' 数据库和几何操作
Imports ZwSoft.ZwCAD.DatabaseServices
Imports ZwSoft.ZwCAD.Geometry
' 用户交互
Imports ZwSoft.ZwCAD.EditorInput
'Imports Autodesk.AutoCAD
'Imports Autodesk.AutoCAD.ApplicationServices
'Imports Autodesk.AutoCAD.Colors
'Imports Autodesk.AutoCAD.DatabaseServices
'Imports Autodesk.AutoCAD.DatabaseServices.Filters
'Imports Autodesk.AutoCAD.EditorInput
''Imports Autodesk.AutoCAD.GraphicsInterface‘注意该空间也有polyline，避免混淆’
'Imports Autodesk.AutoCAD.Geometry
'Imports Autodesk.AutoCAD.GraphicsSystem
'Imports Autodesk.AutoCAD.LayerManager
''//*******************************************//
''//               Type Library                //
''//*******************************************//
''//               acdbmgd.dll                 //
''//*******************************************//
'Imports Autodesk.AutoCAD.Runtime
'Imports Autodesk.AutoCAD.Windows
''//********************************************//
''//--------------------------------------------//
''//                 acmgd.dll                  //
''//--------------------------------------------//
'Imports Autodesk.AutoCAD.Windows.ToolPalette
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Public Class Class_main
    ''Public doc As AcadDocument
    '' 自动执行
    '<CommandMethod("AutoRun")>
    'Public Shared Sub AutoRun()
    '    Dim doc As Document = Application.DocumentManager.MdiActiveDocument
    '    Dim ed As Editor = doc.Editor
    '    ed.WriteMessage(vbLf & "🎉『SITUO 传递系数法』 Getslidedata.dll 已成功加载！")
    '    ed.WriteMessage(vbLf & "📋 可用命令: Getslidedata")
    'End Sub
    <CommandMethod("getdq", CommandFlags.UsePickSet)> Public Sub Test()

        Dim docs As DocumentCollection = Application.DocumentManager
        Dim doc As Document = docs.MdiActiveDocument
        'Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim ed As Editor = doc.Editor
        Dim db As Database = doc.Database

        'Dim pile_x As New List(Of Double)() '记录抗滑桩的X坐标
        'Public doc As AcadDocument

        Dim version_name As String = "『dq挡墙数据提取插件for CAD2014』"
        ed.WriteMessage("####################################" & Environment.NewLine)
        ed.WriteMessage("欢迎使用『dq挡墙数据提取插件for CAD2014』" & Environment.NewLine)
        ed.WriteMessage("####################################" & Environment.NewLine)
        'ed.WriteMessage("开发者：杜金龙 1969399672@QQ.com" & Environment.NewLine)
        'ed.WriteMessage(version_name & Environment.NewLine)


        'Dim number As Integer = 99999999
        'Dim ss_split_line As SelectionSet
        ''Dim ss_water As SelectionSet
        'Dim ss_landslide_point As SelectionSet
        'Dim ss_pile As SelectionSet
        'Dim ss_dim As AcadSelectionSet
        'Dim ss_water As AcadSelectionSet
        'Dim ss_point As AcadSelectionSet

        'Dim flag_water As Boolean ' 默认值为False
        'Dim number_AB_point As Integer = 0
        'Dim output_list As ArrayList = New ArrayList
        Dim arr As ArrayList = New ArrayList
        'Dim dataset_landslide_caldata As System.Data.DataSet = New DataSet
        'Dim dataset_geometry_data As DataSet = New DataSet '存储点位坐标信息的各个datatable，如果没有没有地下水，则没有必要使用，因为只有一个datatable

        'dt_landslide_AB_point
        Dim dataset_DQ As DataSet = New DataSet

        Dim datatable_T As System.Data.DataTable = New System.Data.DataTable '注意这是一个仅仅存储滑坡前后缘点的datable。但是其结构和存储整个滑坡坐标的datatable相同，也可以使用其结构
        datatable_T.Columns.Add("ID", System.Type.GetType("System.String"))
        datatable_T.Columns.Add("x", System.Type.GetType("System.Double"))
        datatable_T.Columns.Add("y", System.Type.GetType("System.Double")) '顶

        Dim datatable_B As System.Data.DataTable = New System.Data.DataTable '注意这是一个仅仅存储滑坡前后缘点的datable。但是其结构和存储整个滑坡坐标的datatable相同，也可以使用其结构
        datatable_B.Columns.Add("ID", System.Type.GetType("System.String"))
        datatable_B.Columns.Add("x", System.Type.GetType("System.Double"))
        datatable_B.Columns.Add("y", System.Type.GetType("System.Double")) '顶



        ed.WriteMessage(Environment.NewLine & version_name & "第1步:请选择挡墙顶部地形线!" & Environment.NewLine)
        ' 1. 设置选择选项
        Dim options As New PromptEntityOptions(vbLf & "请选择一条多段线: ")
        ' 2. 关键代码：限制只能选择 Polyline 类型的对象
        options.SetRejectMessage(vbLf & "无效的对象，请选择多段线。")
        options.AddAllowedClass(GetType(Polyline), False)

        ' 3. 执行单选操作

        Dim result As PromptEntityResult = ed.GetEntity(options) 'GetEntity 则专门用于让用户在屏幕上点选单个实体。
        'Editor.GetEntity()(推荐) 强制单选 返回PromptEntityResult
        'Editor.SelectAll()/Select() 可选择多个对象  返回值PromptSelectionResult
        If result.Status <> PromptStatus.OK Then Return

        Dim polyId As ObjectId = result.ObjectId

        'Dim id As ObjectId = objectIds_split_line(j)
        Dim ent_dim As Entity = Get_entity_by_objectid(db, polyId)
        Dim lint_temp As Polyline = ent_dim

        For n = 0 To lint_temp.NumberOfVertices - 1
            Dim p3d As Point3d = lint_temp.GetPoint3dAt(n)
            Dim dr As DataRow = datatable_T.NewRow
            dr("ID") = n
            dr("x") = p3d.X
            dr("y") = p3d.Y
            datatable_T.Rows.Add(dr)
        Next



        dataset_DQ.Tables.Add(datatable_T)
        arr.Add(" 挡墙顶部地形线")



        '#####底部’
        ed.WriteMessage(Environment.NewLine & version_name & "第2步:请选择挡墙底部地形线!" & Environment.NewLine)
        ' 1. 设置选择选项
        Dim options2 As New PromptEntityOptions(vbLf & version_name & "请选择一条多段线: ")
        ' 2. 关键代码：限制只能选择 Polyline 类型的对象
        options2.SetRejectMessage(vbLf & version_name & "无效的对象，请选择多段线。")
        options2.AddAllowedClass(GetType(Polyline), False)

        ' 3. 执行单选操作

        Dim result2 As PromptEntityResult = ed.GetEntity(options2) 'GetEntity 则专门用于让用户在屏幕上点选单个实体。
        'Editor.GetEntity()(推荐) 强制单选 返回PromptEntityResult
        'Editor.SelectAll()/Select() 可选择多个对象  返回值PromptSelectionResult
        If result2.Status <> PromptStatus.OK Then Return

        Dim polyId2 As ObjectId = result2.ObjectId

        'Dim id As ObjectId = objectIds_split_line(j)
        Dim ent_dim2 As Entity = Get_entity_by_objectid(db, polyId2)
        Dim lint_temp2 As Polyline = ent_dim2

        For n = 0 To lint_temp2.NumberOfVertices - 1
            Dim p3d As Point3d = lint_temp2.GetPoint3dAt(n)
            Dim dr As DataRow = datatable_B.NewRow
            dr("ID") = n
            dr("x") = p3d.X
            dr("y") = p3d.Y
            datatable_B.Rows.Add(dr)
        Next



        dataset_DQ.Tables.Add(datatable_B)
        arr.Add("挡墙底部地形线")

        'dataset_geometry_data.Tables.Add(dt_temp)


        'Dim str_file As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(db.Filename), "剖面" & DateTime.Now.ToString("hh:mm:ss") & ".xls")
        Dim str_file As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(db.Filename), "挡墙地形线数据" & DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") & ".xlsx")
        'Dim str_file2 As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(db.Filename), "挡墙几何图形数据" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".xlsx")
        'HH为24小时，hh为12小时，文件名不能有：冒号yyyyMMdd
        'Application.ShowAlertDialog(dataset_landslide_caldata.Tables.Count)
        'Application.ShowAlertDialog(dataset_geometry_data.Tables.Count)
        'Export_dataset_to_excel(dataset_landslide_caldata, str_file, arr)

        'ExportDataTableToExcel(dt_temp, str_file)
        'SaveDataTableToCsv(dt_temp, str_file)
        'SaveDataTableToExcel(dt_temp, str_file)
        'Export_dataset_to_excel(dataset_geometry_data, str_file, arr)
        'myexcelHelper.ExportToExcelWithStyle(dt_temp, str_file)
        'ExportDataTableToExcel(dt_temp, str_file)
        Export_dataset_to_excel(dataset_DQ, str_file, arr)
        ed.WriteMessage(version_name & "###挡墙地形线数据导出完毕！###" & Environment.NewLine)
        ed.WriteMessage(version_name & "文件保存位置：" & str_file & Environment.NewLine)
        'ed.WriteMessage(version_name & "文件保存位置：" & str_file & Environment.NewLine)
        'ed.WriteMessage(version_name & "文件保存位置：" & str_file2 & Environment.NewLine)
        'Catch ex As system.Exception
        '    LogError(ex) ' 记录错误
        '    Application.ShowAlertDialog($"崩溃: {ex.Message}")
        'End Try
    End Sub

    'Private Sub LogError(ex As System.Exception)
    '    File.AppendAllText("C:\CAD_Errors.log",
    '    $"[{DateTime.Now}] {ex.ToString()}{vbCrLf}{vbCrLf}")
    'End Sub


    Public Function Get_entity_by_objectid(ByVal db As Database, ByVal id As ObjectId) As Entity
        Using acTrans As Transaction = db.TransactionManager.StartTransaction()
            Dim dbo As Entity = acTrans.GetObject(id, OpenMode.ForRead)
            acTrans.Commit()
            Return dbo
        End Using
        'Return Nothing
    End Function

    'Public Sub Create_Layer(ByVal acdoc As Document, ByVal acCurDb As Database)
    '    '' 获取当前文档和数据库
    '    'Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
    '    'Dim acCurDb As Database = acDoc.Database
    '    '' 启动事务
    '    Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '        '' 以读模式打开图层表
    '        Dim acLyrTbl As LayerTable
    '        acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)

    '        Dim sLayerName As String = "ABC"
    '        Dim acLyrTblRec As LayerTableRecord
    '        If acLyrTbl.Has(sLayerName) = False Then
    '            acLyrTblRec = New LayerTableRecord()
    '            '' 给图层名赋值
    '            acLyrTblRec.Name = sLayerName
    '            '' 以写模式升级打开图层表
    '            acLyrTbl.UpgradeOpen()
    '            '' 添加新图层到图层表，记录事务
    '            acLyrTbl.Add(acLyrTblRec)
    '            acTrans.AddNewlyCreatedDBObject(acLyrTblRec, True)
    '        Else
    '            acLyrTblRec = acTrans.GetObject(acLyrTbl(sLayerName), _
    '            OpenMode.ForWrite)
    '        End If


    '        If acLyrTbl.Has(sLayerName) = True Then
    '            ''设置图层 Center 为当前图层
    '            acCurDb.Clayer = acLyrTbl(sLayerName)
    '            'acdoc. = doc.Layers.Item("滑坡条分编号_DUJINLONG")
    '        End If


    '        '' 锁定图层
    '        acLyrTblRec.IsLocked = True
    '        '' 保存修改，关闭事务
    '        acTrans.Commit()
    '    End Using
    'End Sub

    'Public Sub Create_TEXT(ByVal acdoc As Document, ByVal acCurDb As Database, ByVal datatable_temp As System.Data.DataTable)


    '    '' 获取当前文档及数据库

    '    'Dim acDoc As Document = Application.DocumentManager.MdiActiveDocument
    '    'Dim acCurDb As Database = acDoc.Database
    '    '' 启动事务
    '    Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '        '' 以读模式打开 Block 表
    '        Dim acBlkTbl As BlockTable
    '        acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
    '        OpenMode.ForRead)
    '        ''以写模式打开 Block 表记录 Model 空间
    '        Dim acBlkTblRec As BlockTableRecord
    '        acBlkTblRec = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace),
    '        OpenMode.ForWrite)



    '        For i = 0 To datatable_temp.Rows.Count - 1

    '            Dim x As Double = (datatable_temp.Rows(i)("x") + datatable_temp.Rows(i + 1)("x")) / 2
    '            Dim y As Double = (datatable_temp.Rows(i)("y1") + datatable_temp.Rows(i)("y2") + datatable_temp.Rows(i + 1)("y1") + datatable_temp.Rows(i + 1)("y2")) / 4

    '            'Dim startpoint(2) As Double
    '            '        startpoint(0) = (dataset_temp.Tables(0).Rows(ii)("xx") + dataset_temp.Tables(0).Rows(ii + 1)("xx")) / 2
    '            '        startpoint(1) = (dataset_temp.Tables(0).Rows(ii)("y1") + dataset_temp.Tables(0).Rows(ii)("y2") + dataset_temp.Tables(0).Rows(ii + 1)("y1") + dataset_temp.Tables(0).Rows(ii + 1)("y2")) / 4

    '            '' 创建一个单行文字对象
    '            Dim acText As DBText = New DBText()
    '            'acText.Position = New Point3d(2, 2, 0)
    '            acText.Position = New Point3d(x, y, 0)
    '            acText.Height = 2
    '            'acText.TextString = "Hello,World."
    '            acText.TextString = i + 1
    '            acBlkTblRec.AppendEntity(acText)
    '            acTrans.AddNewlyCreatedDBObject(acText, True)
    '            '' 保存修改，关闭事务
    '        Next
    '        acTrans.Commit()
    '    End Using

    'End Sub
    Public Function Is_straight_line(ByVal ent As Entity) As Boolean
        Dim ent_dim As Entity = ent
        '检查剖面线是否为一条竖直线’
        Dim X1_temp As Double
        Dim X2_temp As Double
        If TypeOf ent_dim Is Line Then
            Dim lint_temp As Line = ent_dim
            X1_temp = lint_temp.StartPoint.X
            X2_temp = lint_temp.EndPoint.X

        ElseIf TypeOf ent_dim Is Polyline Then
            Dim lint_temp As Polyline = ent_dim
            X1_temp = lint_temp.StartPoint.X
            X2_temp = lint_temp.EndPoint.X
        Else
            Return False
            Exit Function
        End If

        If X1_temp <> X2_temp Then
            Return False
            ''Application.ShowAlertDialog("发现不合格剖面线（剖面线不竖直）!不影响程序运行。。。") '如果用框选的方法，则此处多余，因为可能是水位线和地面线和滑带
            'Continue For '执行下一个循环，不要用exit for
        Else
            'If pile_x.Contains(X1_temp) = False Then
            '    pile_x.Add(X1_temp)
            'End If\
            Return True
        End If
    End Function


    Public Sub LogError(ex As System.Exception,
                  <CallerLineNumber> Optional lineNumber As Integer = 0,
                  <CallerMemberName> Optional methodName As String = "",
                  <CallerFilePath> Optional filePath As String = "")

        Dim logText = $"错误位置: {methodName} (行号: {lineNumber}){vbCrLf}" &
                  $"详细错误: {ex.ToString()}"

        ' 写入日志文件
        File.AppendAllText(" D:\CAD_Errors.log", logText)

        ' 显示在CAD命令行
        Try
            Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
            ed.WriteMessage($"{vbCrLf}错误发生在: {methodName} 第{lineNumber}行{vbCrLf}")
        Catch
        End Try
    End Sub
End Class
