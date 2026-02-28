import { useAppSelector, useAppDispatch } from '@/store/hooks';
import { RootState } from '@/store/store';
import {
    setActiveDir,
    setActiveMode,
    setActiveTheme,
    setActiveLayout,
    setIsCardShadow,
    setIsLayout,
    setIsBorderRadius,
    setIsCollapse,
    setIsLanguage,
    setIsSidebarHover,
    setIsMobileSidebar,
} from '@/store/slices/appSlice';

import { AppState } from '@/store/slices/appSlice';

export const useCustomizer = () => {
    const customizer = useAppSelector((state: RootState) => state.app) as AppState;
    const dispatch = useAppDispatch();

    return {
        ...customizer,
        setActiveDir: (val: string) => dispatch(setActiveDir(val)),
        setActiveMode: (val: string) => dispatch(setActiveMode(val)),
        setActiveTheme: (val: string) => dispatch(setActiveTheme(val)),
        setActiveLayout: (val: string) => dispatch(setActiveLayout(val)),
        setIsCardShadow: (val: boolean) => dispatch(setIsCardShadow(val)),
        setIsLayout: (val: string) => dispatch(setIsLayout(val)),
        setIsBorderRadius: (val: number) => dispatch(setIsBorderRadius(val)),
        setIsCollapse: (val: string) => dispatch(setIsCollapse(val)),
        setIsLanguage: (val: string) => dispatch(setIsLanguage(val)),
        setIsSidebarHover: (val: boolean) => dispatch(setIsSidebarHover(val)),
        setIsMobileSidebar: (val: boolean) => dispatch(setIsMobileSidebar(val)),
    };
};
