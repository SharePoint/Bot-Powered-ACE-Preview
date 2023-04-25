import { ICardActionParameters } from "./ICardActionParameters";

export interface ISubmitCardParameters extends ICardActionParameters{
    /**
     * Key value pair property that can be defined for submit card action parameters.
     */
    [key: string]: unknown;
}