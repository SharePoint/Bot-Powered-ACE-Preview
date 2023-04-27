import { ICardActionParameters } from "./ICardActionParameters";

export interface IExecuteCardParameters extends ICardActionParameters {
    
    /**
     * Key value pair property that can be defined for execute card action parameters.
     */
    [key: string]: unknown;
}