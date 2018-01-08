import 'colors';

var succeeded = 0;
var failed = 0;

/**
 * Defines a suite of unit tests.
 * @param name The name of this suite of tests.
 * @param tests A function that runs the tests.
 */
export function suite(name: string, tests: () => void) {
    console.log(name.bold.blue);
    tests();
}

/**
 * Defines a unit test.
 * @param description A description of the test being run.
 * @param test The test to run.
 */
export function test(description: string, test: () => void) {
    try {
        test();
        console.log(`\t${description.green}`);
        succeeded++;
    }
    catch (exception) {
        console.log(`\t${description.red}`);
        failed++;
    }
}

/**
 * Prints the overall results of this unit test session.
 */
export function results() {
    console.log(`${succeeded} / ${succeeded + failed} passing.`.magenta);
    if (!failed) console.log('All tests passed!'.bold.yellow);
}
