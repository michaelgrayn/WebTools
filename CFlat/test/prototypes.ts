import 'should'
import { suite, test } from  '../source/unit-tests/suite';

export module prototype_extensions {
    suite('String.isBlank', () => {
        test('Empty String', () => {
            ''.isBlank().should.equal(true);
        });
        test('Whitespace', () => {
            '  '.isBlank().should.equal(true);
        });
        test('String With Content', () => {
            'NotBlank'.isBlank().should.equal(false);
        });
    });
    
    const array = [1, 2, 3];

    suite('Array.any', () => {
        test('Empty Array', () => {
            [].any().should.equal(false);
        });
        test('Array With Elements', () => {
            array.any().should.equal(true);
        });
    });

    suite('Array.first and Array.last', () => {
        test('First', () => {
            array.first().should.equal(1).and.be.a.Number();
        });
        test('Last', () => {
            array.last().should.equal(3).and.be.a.Number();
        });
    });

    suite('Array.spliceAt', () => {
        test('Return Value', () => {
            array.spliceAt(1).should.equal(2).and.be.a.Number();
        });
        test('Resulting Array', () => {
            array.should.deepEqual([1, 3]).and.be.an.Array();
        });
    });

    suite('Array.popElements', () => {
        array.push(1, 2);
        test('Return Value', () => {
            array.popElements(1, 2).should.equal(1).and.be.a.Number();
        });
        test('Resulting Array', () => {
            array.should.deepEqual([3]).and.be.an.Array();
        });
    });
}
