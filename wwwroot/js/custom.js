let index = 0;





function AddTag() {
    //Get TagEntry from the document
    let tagEntry = document.getElementById("TagEntry");

    //Create a new Select Option
    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.getElementById("TagList").options[index++] = newOption;

    //Clear out the TagEntry control
    tagEntry.value = "";

    return true;
}

function RemoveTag() {
    let tagCount = 1;
    while (tagCount > 0) {
        let tagList = document.getElementById("TagList")
        let selectedIndex = tagList.selectedIndex;
        if (selectedIndex >= 0) {
            tagList.options[selectedIndex] = null;
            --tagCount;
            --index;
        }
        else {
            tagCount = 0;
            index--;
        }
    }
}

$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
})

//Check if the tagValues variable has data
if (tagValues != "") {
    let tagArray = tagValues.split(",");
    for (let loop = 0; loop < tagArray.length; loop++) {
        //Load or replace the options that we have
        replaceTag(tagArray[loop], loop);
        index++;
    }
}

function replaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}