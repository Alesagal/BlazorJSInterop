function invokeAsync(testObject) {
    testObject.number = -20;
    return testObject;
}

function invokeVoidAsync(testObject) {
    console.log(testObject);
}

function empty(s) {
    console.log('empty', s);
}