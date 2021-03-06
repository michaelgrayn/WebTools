interface String {
    /**
     * Checks if the string is undefined, null, empty, or whitespace.
     */
    isBlank(): boolean;
}

interface Array<T> {
    /**
     * Determines whether the array contains any elements.
     */
    any(): boolean;

    /**
     * Gets the first element of the array or undefined if empty.
     */
    first(): T;

    /**
     * Gets the last element of the array or undefined if empty.
     */
    last(): T;

    /**
     * Removes elements form the array, and returns the new length of the array.
     * @param items The elements to remove.
     */
    popElements(...items: T[]): number;

    /**
     * Removes the element at the given index and returns it.
     * @param index The zero-based location in the array of the element to remove.
     */
    spliceAt(index: number): T;
}
