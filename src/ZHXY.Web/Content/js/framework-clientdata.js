$(function () {
    window.clients = $.clientsInit();
    localStorage.clear();
    for (var k in clients) {
        localStorage.setItem(k, JSON.stringify(clients[k]));
    }
})

function getLimYear(data) {
    var output = {}
    var currentYear = new Date().getFullYear()
    var yearRange = []
    for (i = 0; i < 5; i++) {
        yearRange.push(currentYear + i)
    }
    $.each(data, function (n, t) {
        $.each(yearRange, function (i, item) {
            if (Number(n) === item) {
                output[n] = t
            }
        })
    })
    return output
}

$.clientsInit = function () {
    let dataJson = {
        dataItems: [],
        organize: [],
        role: [],
        duty: [],
        user: [],
        authorizeMenu: [],
        area: [],
        schoolarea: [],
        semester: [],
        authorizeButton: [],
        orgDic: [],
        classTeachers: [],
        department: {
            //精品小学
            '1': ['F_Sco_Pir', 'F_Sco_Line', 'F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Sco_Line', 'F_Sco_Pir', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //精品初中
            '3': ['F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3',],
            //精品高中
            '5': ['F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //国际小学
            '2': ['F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //国际初中
            '4': ['F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //国际高中
            '6': ['F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //国际留学生
            '7': ['F_Sco_Pir', 'F_Sco_Line', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //培训中心
            '8': ['F_Relative1_Guarder_Relation', 'F_Relative1_Name', 'F_Relative1_Comp', 'F_Relative1_Guarder', 'F_Relative1_Tel', 'F_Relative2_Guarder_Relation', 'F_Relative2_Tel', 'F_Relative2_Name', 'F_Relative2_Comp', 'F_Relative2_Guarder', 'F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Guarder_Nation', 'F_Guarder_CredNum', 'F_Guarder_CredType', 'F_Guarder_CredPhoto_Obve', 'F_Guarder_CredPhoto_Rever', 'F_Reg_Status', 'F_RegAddr', 'F_RegRelat', 'F_RegMainName', 'F_RegPhoto_Obve', 'F_RegPhoto_Rever', 'F_Height', 'F_Weight', 'F_Blood_Type', 'F_Allergy', 'F_Food', 'F_MedicalHis', 'F_MedicalHis_Memo', 'F_Name', 'F_Gender', 'F_Name_Old ', 'F_Name_En', 'F_Birthday', 'F_Only_One', 'F_Nation', 'F_PolitStatu', 'F_Native', 'F_Volk', 'F_CredType', 'F_CredNum', 'F_CredPhoto_Obve', 'F_CredPhoto_Rever', 'F_Home_Addr', 'F_Sco_Pir', 'F_Sco_Line', 'F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_MedicalHis_Memo', 'F_MedicalHis', 'F_Food', 'F_Allergy', 'F_Blood_Type', 'F_Weight', 'F_Height', 'F_Guarder_LinkType', 'F_Relative3_Guarder_Relation', 'F_Relative3_Name', 'F_Relative3_Tel', 'F_Relative3_Guarder3', 'F_Relative3_Comp3', 'F_Eat', 'F_Relish', 'F_Incontinence', 'F_Dress', 'F_Sleep', 'F_Stripped', 'F_Physique', 'F_Life_Memo'],
            //幼儿园
            '9': ['F_Guarder_Relation', 'F_Guarder', 'F_Guarder_Dw', 'F_Guarder_Wh', 'F_Guarder_Tel', 'F_Native_Old', 'F_Cn_Level', 'F_HSK_TEST', 'F_Stu_Time', 'F_Guarder_LinkType'],
        }
    };
    (() => {
        $.ajax({
            url: "/ClientData/Get?clientType=1",
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                dataJson.dataItems = data.dataItems;
                dataJson.organize = data.organize;
                dataJson.role = data.role;
                dataJson.duty = data.duty;
                dataJson.area = data.area;
                dataJson.schoolarea = data.schoolarea;
                dataJson.authorizeMenu = eval(data.authorizeMenu);
                dataJson.authorizeButton = data.authorizeButton;
                dataJson.areachild = data.areachild;
                dataJson.schoolareachild = data.schoolareachild;
                dataJson.dataItems.limYear = getLimYear(dataJson.dataItems.F_Year)
                dataJson.semester = data.semester;
                dataJson.devices = data.devices;
                dataJson.course = data.course;
                dataJson.classTeachers = data.classTeachers;
                dataJson.schedulestime = data.schedulestime;
            }
        });
    })();
    return dataJson;
}