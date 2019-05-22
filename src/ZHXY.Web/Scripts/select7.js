
const yujf = {

    select7: function (data) {
        let options = [`<option>Please Chose</option>`];
        for (let k in data) {
            let option = `<option value='${k}'>${data[k]}</option>`;
            options.push(option);
        }
        return options;
    },

    getItem: function (key) {
        return JSON.parse(localStorage[key]);
    },

    getDic: function (dicName) {
        return getItem("dic")[dicName];
    },

    getDicValue: function (dicName, dicKey) {
        return getDic(dicName)[dicKey] == undefined ? "" : window.getDic(dicName)[dicKey];
    }
}
