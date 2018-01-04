import { expect } from "chai";
import "../src/index.cb.js"

describe("string.isBlank", () => {
    it("empty string, whitespace, and string with text", () => {
        expect(''.isBlank()).is.true;
        expect('  '.isBlank()).is.true;
        expect('NotBlank'.isBlank()).is.false;
    });
});

const array = [1, 2, 3];

describe("array.first", () => {
    it("get first value of array", () => {
        expect(array.first()).to.eq(1)
    });
});

describe("array.last", () => {
    it("get last value of array", () => {
        expect(array.last()).to.eq(3)
    });
});
