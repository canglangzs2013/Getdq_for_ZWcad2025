
Module Module_dt_hp
    Function Get_dt_hp() As DataTable
        Dim dt_hp_temp As System.Data.DataTable = New System.Data.DataTable
        dt_hp_temp.Columns.Add("ID", System.Type.GetType("System.String"))
        'dt_hp_temp.Columns.Add("ID", System.Type.GetType("System.Int32"))
        'dt_hp_temp.Columns("ID").AllowDBNull = False
        'dt_hp_temp.Columns("ID").AutoIncrement = True
        'dt_hp_temp.Columns("ID").AutoIncrementStep = 1
        'dt_hp_temp.Columns("ID").AutoIncrementSeed = 1
        dt_hp_temp.Columns("ID").Caption = "条块号"

        dt_hp_temp.Columns.Add("A", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("A").Caption = "条块面积(m2)"
        dt_hp_temp.Columns.Add("A1", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("A1").Caption = "水位以上面积Wi1(m2)"
        'dt_hp_temp.Columns.Add("A2", System.Type.GetType("System.Double"))
        'dt_hp_temp.Columns("A2").Caption = "降水入渗面积Wi2(m2)"
        dt_hp_temp.Columns.Add("A2", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("A2").Caption = "水位以下面积Wi2(m2)"
        'dt_hp_temp.Columns.Add("A4", System.Type.GetType("System.Double"))
        'dt_hp_temp.Columns("A4").Caption = "条块以上水体面积Wi4(m2)"


        dt_hp_temp.Columns.Add("angle", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("angle").Caption = "滑面倾角(°)"
        dt_hp_temp.Columns.Add("L", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("L").Caption = "滑面长(m)"

        dt_hp_temp.Columns.Add("Hb", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("Hb").Caption = "靠山侧水位Hb(m)"
        dt_hp_temp.Columns.Add("Ha", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("Ha").Caption = "背山侧水位Ha(m)"

        dt_hp_temp.Columns.Add("angle_B", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("angle_B").Caption = "水力坡度角(°)"


        dt_hp_temp.Columns.Add("r1", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("r1").Caption = "天然重度(kN/m3)"
        dt_hp_temp.Columns.Add("r2", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("r2").Caption = "饱和重度(kN/m3)"

        dt_hp_temp.Columns.Add("c", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("c").Caption = "内聚力(kPa)"
        dt_hp_temp.Columns.Add("fai", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("fai").Caption = "内摩擦角(°)"

        dt_hp_temp.Columns.Add("W", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("W").Caption = "条块重量(kN/m)"
        dt_hp_temp.Columns.Add("load", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("load").Caption = "建筑荷载(kN/m)"

        dt_hp_temp.Columns.Add("earthquake", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("earthquake").Caption = "地震水平系数"

        'dt_hp_temp.Columns.Add("WaterP", System.Type.GetType("System.Double"))
        'dt_hp_temp.Columns("WaterP").Caption = "静水压力合力ΔP(KN/m)"
        'dt_hp_temp.Columns.Add("WaterU", System.Type.GetType("System.Double"))
        'dt_hp_temp.Columns("WaterU").Caption = "底部孔隙水压力Ui(KN/m)"
        dt_hp_temp.Columns.Add("F", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("F").Caption = "浮力Fi(KN/m)"
        dt_hp_temp.Columns.Add("N", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("N").Caption = "法向分力Ni(KN/m)"
        dt_hp_temp.Columns.Add("R", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("R").Caption = "抗滑力(KN/m)"
        dt_hp_temp.Columns.Add("T", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("T").Caption = "下滑力(KN/m)"

        'dt_hp_temp.Columns.Add("DR", System.Type.GetType("System.Double"))
        'dt_hp_temp.Columns("DR").Caption = "累计抗滑力(KN/m)"
        'dt_hp_temp.Columns.Add("DT", System.Type.GetType("System.Double"))
        'dt_hp_temp.Columns("DT").Caption = "累计下滑力(KN/m)"

        dt_hp_temp.Columns.Add("tfactor", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("tfactor").Caption = "传递系数"

        dt_hp_temp.Columns.Add("Fs_current", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("Fs_current").Caption = "稳定系数"


        dt_hp_temp.Columns.Add("E_res", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("E_res").Caption = "剩余下滑力(KN/m)"

        dt_hp_temp.Columns.Add("safe_factor", System.Type.GetType("System.Double")) '1.2’
        dt_hp_temp.Columns("safe_factor").Caption = "安全系数"

        dt_hp_temp.Columns.Add("E_design", System.Type.GetType("System.Double"))
        dt_hp_temp.Columns("E_design").Caption = "设计剩余下滑力(KN/m)"

        Return dt_hp_temp
    End Function

End Module
