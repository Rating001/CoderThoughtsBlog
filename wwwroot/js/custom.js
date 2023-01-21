let index = 0;

const swalWithDarkButton = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-danger btn-sm btn-outline-dark swalButton'
    },
    imageUrl: '/images/error.jpg',
    timer: 5000,
    imageWidth: 200,
    imageHeight: 200,
    buttonsStyling: false
});





function AddTag() {
    //Get TagEntry from the document
    let tagEntry = document.getElementById("TagEntry");

    //Use the search function to detect blank or duplicate errors
    let searchResult = search(tagEntry.value);

    if (searchResult != null) {
        //Trigger the empty tag sweet alert
        swalWithDarkButton.fire({
            html: `<span class="font-weight-bolder">${searchResult.toUpperCase()}</span>`
        });



    } else {
        //Create a new Select Option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("TagList").options[index++] = newOption;

        //Clear out the TagEntry control
        tagEntry.value = "";
    }



    return true;
}

function RemoveTag() {
    let tagCount = 1;
    let tagList = document.getElementById("TagList");
    if (!tagList) return false;

    if (tagList.selectedIndex == -1) {
        swalWithDarkButton.fire({
            html: `<span class="font-weight-bolder">CHOOSE A TAG BEFORE PRESSING REMOVE</span>`
        });
        return true;
    }

    while (tagCount > 0) {
        if (tagList.selectedIndex >= 0) {
            tagList.options[tagList.selectedIndex] = null;
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

//The search function will detect an empty or duplicate Tag 
//on the same post and return an error string if an error is detected
function search(str) {
    if (str == "") {
        return "Empty tags are not permitted.";
    }

    var tagsElement = document.getElementById("TagList");

    if (tagsElement) {
        let options = tagsElement.options;
        for (let i = 0; i < options.length; i++) {
            if (options[i].value == str) {
                return `The Tag #${str} is a duplicate and will not be added.`
            }

        }
    }

}

