#import <UIKit/UIKit.h>
#import<AssetsLibrary/AssetsLibrary.h>
#define SYSTEM_VERSION_LESS_THAN( v )  ( [[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedAscending )

@interface ViewController : UIViewController
-(id)init;

@end

@implementation ViewController

-(id)init {
    return [super init];
}


@end


extern "C"
{

    int GallerySave(char* path);
void setPushModel()
{
    NSLog(@"set:PushModel");
}

void shareFb()
{
    NSLog(@"shareFb");
}

void setLbsDetail(char* name, char* pageId) {
    NSLog(@"setLbsDetail");
}

void getLbsApi(char* linkName){
    NSLog(@"lbsApi");
}

void setLbsInit() {
    NSLog(@"setLbsInit");
}

}



int GallerySave(char* path)
{
    NSString *imagePath = [NSString stringWithUTF8String:path];
    
    ALAssetsLibrary *lib = [[ALAssetsLibrary alloc] init];
    [lib enumerateGroupsWithTypes:ALAssetsGroupSavedPhotos usingBlock:^(ALAssetsGroup *group, BOOL *stop) {
    } failureBlock:^(NSError *error) { }];
    
    ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
    
    if (status == ALAuthorizationStatusRestricted || status == ALAuthorizationStatusDenied)
    {
        return 2;
    }
    
    if(![[NSFileManager defaultManager] fileExistsAtPath:imagePath])
    {
        return 0;
    }
    
    UIImage *image = [UIImage imageWithContentsOfFile:imagePath];
    
    if(image)
    {
        UIImageWriteToSavedPhotosAlbum( image, nil, NULL, NULL );

        return 1;
    }
    
    return 0;
}


